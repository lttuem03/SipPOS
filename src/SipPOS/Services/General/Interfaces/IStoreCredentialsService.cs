namespace SipPOS.Services.General.Interfaces;

/// <summary>
/// Store credentials saving/loading service. Used to implement the "Save credentials" when logging in.
/// </summary>
public interface IStoreCredentialsService
{
    /// <summary>
    /// Saves the store credentials.
    /// </summary>
    /// <param name="storeUsername">The username of the store.</param>
    void SaveCredentials(string storeUsername, string storePassword);

    /// <summary>
    /// Gets the store credentials.
    /// </summary>
    /// <returns>The store credentials if exists, otherwise (null, null).</returns>
    (string? storeUsername, string? storePassword) LoadCredentials();

    /// <summary>
    /// Clears the stored credentials if exist.
    /// </summary>
    void ClearCredentials();
}