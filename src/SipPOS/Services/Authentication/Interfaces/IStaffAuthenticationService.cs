namespace SipPOS.Services.Authentication.Interfaces;

/// <summary>
/// Service interface for staff authentication.
/// </summary>
public interface IStaffAuthenticationService
{
    /// <summary>
    /// Logs in a staff member asynchronously using the provided composite username and password.
    /// Composite username: [position_prefix][store_id][staff_id]
    /// </summary>
    /// <param name="username">The composite username of the staff member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the login was successful.</returns>
    /// <remarks>To login to a staff account, you don't need the password. But, to open a shift for that staff, you'll need password.</remarks>
    Task<(bool succeded, string? errorMessage)> LoginAsync(string compositeUsername);

    /// <summary>
    /// Logs out the current staff member.
    /// </summary>
    void Logout();
}
