namespace SipPOS.Services.Authentication.Interfaces;

/// <summary>
/// Service interface for staff authentication.
/// </summary>
public interface IStaffAuthenticationService
{
    /// <summary>
    /// Logs in a staff member asynchronously using the provided composite username.
    /// Composite username: [position_prefix][store_id][staff_id]
    /// </summary>
    /// <param name="compositeUsername">The composite username of the staff member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple indicating whether the login was successful and an error message if it failed.</returns>
    /// <remarks>To login to a staff account, you don't need the password. But, to open a shift for that staff, you'll need password.</remarks>
    Task<(bool succeded, string? errorMessage)> LoginAsync(string compositeUsername);

    /// <summary>
    /// Verifies the password of a staff member asynchronously.
    /// </summary>
    /// <param name="compositeUsername">The composite username of the staff member.</param>
    /// <param name="password">The password to verify.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple indicating whether the verification was successful and an error message if it failed.</returns>
    Task<(bool succeded, string? errorMessage)> VerifyPasswordAsync(string compositeUsername, string password);

    /// <summary>
    /// Logs out the current staff member.
    /// </summary>
    void Logout();
}
