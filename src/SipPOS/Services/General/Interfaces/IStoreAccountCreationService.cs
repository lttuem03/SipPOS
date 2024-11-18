using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.DataTransfer.Entity;
using SipPOS.Models.Entity;

namespace SipPOS.Services.General.Interfaces;

/// <summary>
/// Service interface for creating store accounts.
/// </summary>
public interface IStoreAccountCreationService
{
    /// <summary>
    /// Creates a new store account asynchronously.
    /// </summary>
    /// <param name="storeDto">The data transfer object containing raw store details (the fields the user entered).</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created store 
    /// if successful (to be authenticated immediately); otherwise, null.</returns>
    Task<Store?> CreateAccountAsync(StoreDto storeDto);

    /// <summary>
    /// Validates the fields of the store data transfer object.
    /// </summary>
    /// <param name="storeDto">The data transfer object containing store details (the fields the user entered).</param>
    /// <returns>A dictionary containing validation results, where the key is the field name and the value is the error message or "OK".</returns>
    Dictionary<string, string> ValidateFields(StoreDto storeDto);
}
