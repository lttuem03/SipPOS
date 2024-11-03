using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.DataTransfer;

namespace SipPOS.DataAccess.Interfaces;

public interface IStoreDao
{
    // Every methods here return nullable objects
    // meaning that if they returned null, then some
    // data access operation has failed

    // Insert operation usually used for registering a new store,
    // then log them in immediately. So here we return the actual
    // Store instance that will be logged in.
    Task<Store?> InsertAsync(StoreDto storeDto);
    Task<StoreDto?> GetByIdAsync(int id);
    Task<StoreDto?> GetByUsernameAsync(string username);
    Task<StoreDto?> UpdateByIdAsync(int id, StoreDto updatedStoreDto);
    Task<StoreDto?> UpdateByUsernameAsync(string username, StoreDto updatedStoreDto);
    Task<StoreDto?> DeleteByIdAsync(int id);
    Task<StoreDto?> DeleteByUsernameAsync(string username);
}