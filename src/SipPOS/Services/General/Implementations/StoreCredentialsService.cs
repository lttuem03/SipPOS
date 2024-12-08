using Windows.Storage;
using Windows.Security.Credentials;

using SipPOS.Services.General.Interfaces;

namespace SipPOS.Services.General.Implementations;

public class StoreCredentialsService : IStoreCredentialsService
{
    // Implemented using: https://learn.microsoft.com/en-us/windows/apps/develop/security/credential-locker
    // and the LocalSettings of Windows.Storage.ApplicationData

    // Though the PasswordVault of Windows.Security.Credentials is not entirely secure
    // (see: https://stackoverflow.com/q/45630304)
    // but in the context of logging a Store into a POS machine running Windows, it is secure enough.

    private const string ResourceName = "SipPOS_StoreCredentials";
    private const string UsernameKey = "Username";

    /// <summary>
    /// Saves the store credentials.
    /// </summary>
    /// <param name="storeUsername">The username of the store.</param>
    /// <param name="storePassword">The password of the store.</param>
    public void SaveCredentials(string storeUsername, string storePassword)
    {
        var localSettings = ApplicationData.Current.LocalSettings;

        var vault = new PasswordVault();
        var credential = new PasswordCredential(ResourceName, storeUsername, storePassword);

        vault.Add(credential);
        localSettings.Values[UsernameKey] = storeUsername;
    }

    /// <summary>
    /// Gets the store credentials.
    /// </summary>
    /// <returns>The store credentials if exists, otherwise (null, null).</returns>
    public (string? storeUsername, string? storePassword) LoadCredentials()
    {
        var localSettings = ApplicationData.Current.LocalSettings;

        if (localSettings.Values[UsernameKey] is not string storeUsername)
        {
            return (null, null);
        }

        var vault = new PasswordVault();

        try
        {
            PasswordCredential credential = vault.Retrieve(ResourceName, storeUsername);

            return (credential.UserName, credential.Password);
        }
        catch (Exception)
        {
            // Credentials not found
            return (null, null);
        }
    }

    /// <summary>
    /// Clears the stored credentials if they exist.
    /// </summary>
    public void ClearCredentials()
    {
        var localSettings = ApplicationData.Current.LocalSettings;

        if (localSettings.Values[UsernameKey] is not string storeUsername)
        {
            return;
        }

        localSettings.Values.Remove(UsernameKey);

        var vault = new PasswordVault();

        try
        {
            PasswordCredential credential = vault.Retrieve(ResourceName, storeUsername);

            vault.Remove(credential);
        }
        catch (Exception)
        {
            // Credentials not found
            return;
        }
    }
}
