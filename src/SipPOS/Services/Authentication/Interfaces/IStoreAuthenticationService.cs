using SipPOS.Models.Entity;

namespace SipPOS.Services.General.Interfaces;

/// <summary>
/// Service interface for store authentication.
/// </summary>
public interface IStoreAuthenticationService
{
    /// <summary>
    /// Gets the current store ID if the store is logged in.
    /// </summary>
    /// <returns>The ID of the current store if store has been authenticated. Otherwise -1.</returns>
    long GetCurrentStoreId();

    /// <summary>
    /// Logs in a store asynchronously using the provided username and password.
    /// </summary>
    /// <param name="username">The username of the store.</param>
    /// <param name="password">The password of the store.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the login was successful.</returns>
    Task<bool> LoginAsync(string username, string password);

    /// <summary>
    /// Logs in a freshly created store asynchronously.
    /// </summary>
    /// <param name="freshlyCreatedStore">The freshly created store.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the login was successful.</returns>
    Task<bool> LoginAsync(Store freshlyCreatedStore); // meant to be used to authenticate a new store after registration

    /// <summary>
    /// Logs out the current store.
    /// </summary>
    void Logout();
}
