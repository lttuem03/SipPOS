using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.DataTransfer.General;

namespace SipPOS.Services.DataAccess.Interfaces;

/// <summary>  
/// Interface for managing special offer data access operations.  
/// </summary>  
public interface ISpecialOfferDao
{
    /// <summary>  
    /// Inserts a new special offer into the database.  
    /// </summary>  
    /// <param name="storeId">The ID of the store.</param>  
    /// <param name="specialOffers">The special offer to insert.</param>  
    /// <returns>The inserted special offer, or null if the insertion failed.</returns>  
    Task<SpecialOffer?> InsertAsync(long storeId, SpecialOffer specialOffers);

    /// <summary>  
    /// Retrieves all special offers for a given store.  
    /// </summary>  
    /// <param name="storeId">The ID of the store.</param>  
    /// <returns>A list of special offers.</returns>  
    Task<List<SpecialOffer>> GetAllAsync(long storeId);

    /// <summary>  
    /// Retrieves a special offer by its ID.  
    /// </summary>  
    /// <param name="storeId">The ID of the store.</param>  
    /// <param name="id">The ID of the special offer.</param>  
    /// <returns>The special offer, or null if not found.</returns>  
    Task<SpecialOffer?> GetByIdAsync(long storeId, long id);

    /// <summary>  
    /// Retrieves special offers with pagination and filtering.  
    /// </summary>  
    /// <param name="storeId">The ID of the store.</param>  
    /// <param name="specialOffersFilterDto">The filter criteria for special offers.</param>  
    /// <param name="sortDto">The sorting criteria.</param>  
    /// <param name="page">The page number.</param>  
    /// <param name="perPage">The number of items per page.</param>  
    /// <returns>A paginated list of special offers.</returns>  
    Task<Pagination<SpecialOffer>> GetWithPaginationAsync
    (
        long storeId,
        SpecialOfferFilterDto specialOffersFilterDto,
        SortDto sortDto,
        int page,
        int perPage
    );

    /// <summary>  
    /// Updates a special offer by its ID.  
    /// </summary>  
    /// <param name="storeId">The ID of the store.</param>  
    /// <param name="specialOffers">The special offer to update.</param>  
    /// <returns>The updated special offer, or null if the update failed.</returns>  
    Task<SpecialOffer?> UpdateByIdAsync(long storeId, SpecialOffer specialOffers);

    /// <summary>  
    /// Deletes a special offer by its ID.  
    /// </summary>  
    /// <param name="storeId">The ID of the store.</param>  
    /// <param name="id">The ID of the special offer to delete.</param>  
    /// <param name="author">The staff member performing the deletion.</param>  
    /// <returns>The deleted special offer, or null if the deletion failed.</returns>  
    Task<SpecialOffer?> DeleteByIdAsync(long storeId, long id, Staff author);

    /// <summary>  
    /// Deletes multiple special offers by their IDs.  
    /// </summary>  
    /// <param name="storeId">The ID of the store.</param>  
    /// <param name="ids">The list of IDs of the special offers to delete.</param>  
    /// <param name="author">The staff member performing the deletion.</param>  
    /// <returns>A list of deleted special offers.</returns>  
    Task<List<SpecialOffer>> DeleteByIdsAsync(long storeId, List<long> ids, Staff author);

    /// <summary>  
    /// Counts the number of special offers that match the given filter criteria.  
    /// </summary>  
    /// <param name="storeId">The ID of the store.</param>  
    /// <param name="specialOffersFilterDto">The filter criteria for special offers.</param>  
    /// <returns>The count of special offers that match the filter criteria.</returns>  
    Task<long> CountAsync(long storeId, SpecialOfferFilterDto specialOffersFilterDto);
}
