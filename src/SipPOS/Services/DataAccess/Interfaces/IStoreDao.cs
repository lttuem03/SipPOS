using SipPOS.DataTransfer.Entity;

namespace SipPOS.Services.DataAccess.Interfaces;

/// <summary>
/// Data Access Object interface for Store data model.
/// </summary>
public interface IStoreDao
{
    // Every methods here return nullable objects
    // meaning that if they returned null, then some
    // data access operation has failed

    // Insert operation usually used for registering a new store,
    // then log them in immediately. So here we return the actual
    // Store instance that will be logged in.

    /// <summary>
    /// Inserts a new store asynchronously.
    /// </summary>
    /// <param name="storeDto">The store data transfer object containing store details.</param>
    /// <returns>The inserted store if successful; otherwise, null.</returns>
    Task<(long id, StoreDto? dto)> InsertAsync(StoreDto storeDto);

    /// <summary>
    /// Retrieves a store by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the store to retrieve.</param>
    /// <returns>The store data transfer object if found; otherwise, null.</returns>
    Task<StoreDto?> GetByIdAsync(long id);

    /// <summary>
    /// Retrieves a store by its username asynchronously.
    /// </summary>
    /// <param name="username">The username of the store to retrieve.</param>
    /// <returns>The store data transfer object if found; otherwise, null.</returns>
    Task<StoreDto?> GetByUsernameAsync(string username);

    /// <summary>
    /// Updates a store by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the store to update.</param>
    /// <param name="updatedStoreDto">The updated store data transfer object.</param>
    /// <returns>The updated store data transfer object if successful; otherwise, null.</returns>
    Task<StoreDto?> UpdateByIdAsync(long id, StoreDto updatedStoreDto);

    /// <summary>
    /// Updates a store by its username asynchronously.
    /// </summary>
    /// <param name="username">The username of the store to update.</param>
    /// <param name="updatedStoreDto">The updated store data transfer object.</param>
    /// <returns>The updated store data transfer object if successful; otherwise, null.</returns>
    Task<StoreDto?> UpdateByUsernameAsync(string username, StoreDto updatedStoreDto);

    /// <summary>
    /// Deletes a store by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the store to delete.</param>
    /// <returns>The deleted store data transfer object if successful; otherwise, null.</returns>
    Task<StoreDto?> DeleteByIdAsync(long id);

    /// <summary>
    /// Deletes a store by its username asynchronously.
    /// </summary>
    /// <param name="username">The username of the store to delete.</param>
    /// <returns>The deleted store data transfer object if successful; otherwise, null.</returns>
    Task<StoreDto?> DeleteByUsernameAsync(string username);
}