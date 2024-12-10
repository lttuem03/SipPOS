using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.Services.Accessibility.Interfaces;

namespace SipPOS.Services.Accessibility.Implementations;

public class PolicyEnforcementPoint : IPolicyEnforcementPoint
{
    private readonly PolicyDecisionPoint _pdp;

    public PolicyEnforcementPoint()
    {
        _pdp = new PolicyDecisionPoint();
    }

    public bool Enforce(Staff staff, Position requiredPosition)
    {
        if (!_pdp.HasRequiredPosition(staff.Position, requiredPosition))
        {
            return false;
        }

        return true;
    }
}