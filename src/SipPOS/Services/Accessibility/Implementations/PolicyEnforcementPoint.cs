using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.Services.Accessibility.Interfaces;

namespace SipPOS.Services.Accessibility.Implementations;

/// <summary>
/// Represents the policy enforcement point which enforces access control policies.
/// </summary>
public class PolicyEnforcementPoint : IPolicyEnforcementPoint
{
    private readonly PolicyDecisionPoint _pdp;

    /// <summary>
    /// Initializes a new instance of the <see cref="PolicyEnforcementPoint"/> class.
    /// </summary>
    public PolicyEnforcementPoint()
    {
        _pdp = new PolicyDecisionPoint();
    }

    /// <summary>
    /// Enforces the required position policy for the given staff.
    /// </summary>
    /// <param name="staff">The staff member to check.</param>
    /// <param name="requiredPosition">The required position to enforce.</param>
    /// <returns><c>true</c> if the staff has the required position; otherwise, <c>false</c>.</returns>
    public bool Enforce(Staff staff, Position requiredPosition)
    {
        if (!_pdp.HasRequiredPosition(staff.Position, requiredPosition))
        {
            return false;
        }

        return true;
    }
}
