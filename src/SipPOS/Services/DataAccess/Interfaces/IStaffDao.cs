using SipPOS.Models.Entity;
using SipPOS.DataTransfer.Entity;

namespace SipPOS.Services.DataAccess.Interfaces;

/// <summary>
/// Interface for Staff Data Access Object
/// </summary>
public interface IStaffDao
{
    // Every methods here return nullable objects
    // meaning that if they returned null, then some
    // data access operation has failed

    // For methods that might return an instance of Staff, not just the Dto,
    // they meant be used for authentication purposes.

    /// <summary>
    /// Inserts a new staff member into the database.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="staffDto">The data transfer object containing staff details.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the inserted StaffDto object, or null if the operation failed.</returns>
    Task<(long id, StaffDto? dto)> InsertAsync(long storeId, StaffDto staffDto);

    /// <summary>
    /// Retrieves all staff members for a specific store.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of StaffDto objects, or null if the operation failed.</returns>
    Task<List<StaffDto>?> GetAllAsync(long storeId);

    Task<(int rowsReturned, List<StaffDto>? staffDtos)> GetWithPagination
    (
        long storeId, 
        int page, 
        int rowsPerPage, 
        string? keyword=null,
        string? sortBy = null,
        string? sortDirection = null
    );

    /// <summary>
    /// Retrieves a staff member by their ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="id">The ID of the staff member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the StaffDto object, or null if the operation failed.</returns>
    Task<StaffDto?> GetByIdAsync(long storeId, long id);

    /// <summary>
    /// Retrieves a staff member by their composite username.
    /// </summary>
    /// <param name="compositeUsername">The composite username of the staff member.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the Staff object, or null if the operation failed.</returns>
    Task<StaffDto?> GetByCompositeUsername(string compositeUsername);

    /// <summary>
    /// Updates a staff member by their ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="id">The ID of the staff member.</param>
    /// <param name="updatedStaffDto">The data transfer object containing updated staff details.</param>
    /// <param name="author">The author of the operation. Presumably is the current staff.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated StaffDto object, or null if the operation failed.</returns>
    Task<StaffDto?> UpdateByIdAsync(long storeId, long id, StaffDto updatedStaffDto, Staff author);

    /// <summary>
    /// Deletes (soft delete - mark the entry as deleted) a staff member by their ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="id">The ID of the staff member.</param>
    /// <param name="author">The author of the operation. Presumably is the current staff.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the deleted StaffDto object, or null if the operation failed.</returns>
    Task<StaffDto?> DeleteByIdAsync(long storeId, long id, Staff author);
}
