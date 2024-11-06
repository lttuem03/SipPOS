using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.Authentication;
using SipPOS.Services.Interfaces;
using SipPOS.DataAccess.Interfaces;

namespace SipPOS.Services.Implementations;

/// <summary>
/// Service for authenticating store accounts. Meant to be used along with a global Store Data Access Object.
/// </summary>
public class StoreAuthenticationService : IStoreAuthenticationService
{
    /// <summary>
    /// Gets the authentication context.
    /// </summary>
    public StoreAuthenticationContext Context { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreAuthenticationService"/> class.
    /// </summary>
    public StoreAuthenticationService()
    {
        Context = new StoreAuthenticationContext();
    }

    /// <summary>
    /// Logs in a store asynchronously using the provided username and password.
    /// </summary>
    /// <param name="username">The username of the store.</param>
    /// <param name="password">The password of the store.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the login was successful.</returns>
    public async Task<bool> LoginAsync(string username, string password)
    {
        var storeDao = App.GetService<IStoreDao>();
        var storeDto = await storeDao.GetByUsernameAsync(username);

        if (storeDto == null) // username not exists in database
        {
            return false;
        }

        // null checks to please the compiler
        if (storeDto.PasswordHash == null ||
            storeDto.Salt == null ||
            storeDto.Id == null ||
            storeDto.Id.HasValue == false)
        {
            return false;
        }

        var passwordEncryptionService = App.GetService<IPasswordEncryptionService>();

        // LOGIN SUCCESSFUL
        if (passwordEncryptionService.Verify(password, storeDto.PasswordHash, storeDto.Salt))
        {
            // Update LastLogin time
            storeDto.LastLogin = DateTime.Now;
            storeDto.UpdatedBy = "system";
            storeDto.UpdatedAt = DateTime.Now;

            var updateResultDto = await storeDao.UpdateByUsernameAsync(username, storeDto);

            if (updateResultDto == null) // something went wrong while updating the LastLogin time
            {
                return false;
            }

            var store = new Store(storeDto.Id.Value, updateResultDto);
            Context.SetStore(store);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Logs in a freshly created store asynchronously.
    /// </summary>
    /// <param name="freshlyCreatedStore">The freshly created store.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the login was successful.</returns>
    public Task<bool> LoginAsync(Store freshlyCreatedStore)
    {
        Context.SetStore(freshlyCreatedStore);

        return Task.FromResult(true);
    }

    /// <summary>
    /// Logs out the current store.
    /// </summary>
    public void Logout()
    {
        if (Context.CurrentStore != null)
        {
            Context.ClearStore();
        }

        return;
    }
}
