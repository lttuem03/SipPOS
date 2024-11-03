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

public class StoreAuthenticationService : IStoreAuthenticationService
{
    public StoreAuthenticationContext Context { get; }

    public StoreAuthenticationService()
    {
        Context = new StoreAuthenticationContext();
    }

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

    public Task<bool> LoginAsync(Store freshlyCreatedStore)
    {
        Context.SetStore(freshlyCreatedStore);

        return Task.FromResult(true);
    }

    public Task LogoutAsync() => throw new NotImplementedException();
}