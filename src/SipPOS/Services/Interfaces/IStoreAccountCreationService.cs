using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.DataTransfer;

namespace SipPOS.Services.Interfaces;

public interface IStoreAccountCreationService
{
    Task<Store?> CreateAccountAsync(StoreDto storeDto);
    Dictionary<string, string> ValidateFields(StoreDto storeDto);
}