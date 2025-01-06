using SipPOS.Models.Entity;
using SipPOS.Models.General;

namespace SipPOS.Services.Accessibility.Interfaces;

/// <summary>
/// Defines the interface for a policy enforcement point.
/// </summary>
public interface IPolicyEnforcementPoint
{
    /// <summary>
    /// Enforces the required position policy for the given staff.
    /// </summary>
    /// <param name="staff">The staff member to check.</param>
    /// <param name="requiredPosition">The required position to enforce.</param>
    /// <returns><c>true</c> if the staff has the required position; otherwise, <c>false</c>.</returns>
    bool Enforce(Staff staff, Position requiredPosition);
}
