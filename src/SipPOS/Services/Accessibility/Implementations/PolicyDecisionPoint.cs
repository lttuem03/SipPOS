using SipPOS.Models.General;
using SipPOS.Services.Accessibility.Interfaces;

namespace SipPOS.Services.Accessibility.Implementations;

/// <summary>
/// Represents the policy decision point which determines if a user has the required position.
/// </summary>
public class PolicyDecisionPoint : IPolicyDecisionPoint
{
    /// <summary>
    /// Determines if the user has the required position.
    /// </summary>
    /// <param name="userPosition">The position of the user.</param>
    /// <param name="requiredPosition">The required position to check against.</param>
    /// <returns>True if the user's position is higher or equal to the required position; otherwise, false.</returns>
    public bool HasRequiredPosition(Position userPosition, Position requiredPosition)
    {
        return userPosition.CompareTo(requiredPosition) >= 0;
    }
}
