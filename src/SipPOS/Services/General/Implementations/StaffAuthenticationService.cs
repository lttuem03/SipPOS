using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Context;
using SipPOS.Context.Authentication;
using SipPOS.Services.General.Interfaces;

namespace SipPOS.Services.General.Implementations;
public class StaffAuthenticationService : IStaffAuthenticationService
{
    public StaffAuthenticationContext Context { get; }

    public StaffAuthenticationService()
    {
        Context = new StaffAuthenticationContext();
    }

    public async Task<bool> LoginAsync(string compositeUsername, string password)
    {
        return false;
    }

    public void Logout()
    {
    
    }
}