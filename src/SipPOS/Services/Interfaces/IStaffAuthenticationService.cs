using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Services.Interfaces;

/// <summary>
/// Service interface for staff authentication.
/// </summary>
public interface IStaffAuthenticationService
{
    /// <summary>
    /// Logs in a staff member asynchronously using the provided username and password.
    /// </summary>
    /// <param name="username">The username of the staff member.</param>
    /// <param name="password">The password of the staff member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the login was successful.</returns>
    Task<bool> LoginAsync(string username, string password);

    /// <summary>
    /// Logs out the current staff member.
    /// </summary>
    void Logout();
}
