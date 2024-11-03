using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models; 

namespace SipPOS.Services.Interfaces;

public interface IStoreAuthenticationService
{
    Task<bool> LoginAsync(string username, string password);
    Task<bool> LoginAsync(Store freshlyCreatedStore); // meant to be used to authenticate a new store after registration
    Task LogoutAsync();
}