using SipPOS.Models.Entity;
using SipPOS.Models.General;

namespace SipPOS.Services.Accessibility.Interfaces;

public interface IPolicyEnforcementPoint
{
    bool Enforce(Staff staff, Position requiredPosition);
}