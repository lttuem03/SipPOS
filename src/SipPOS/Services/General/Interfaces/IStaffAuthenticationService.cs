namespace SipPOS.Services.General.Interfaces;

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
    /// <param name="password">The password of the staff member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the login was successful.</returns>
    Task<bool> LoginAsync(string compositeUsername, string password);

    /// <summary>
    /// Logs out the current staff member.
    /// </summary>
    void Logout();
}
