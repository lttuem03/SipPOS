using SipPOS.Models.General;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;

namespace SipPOS.Services.Entity.Interfaces;

/// <summary>  
/// Interface for managing special offers.  
/// </summary>  
public interface ISpecialOfferService
{
    /// <summary>  
    /// Retrieves special offers with pagination and filtering.  
    /// </summary>  
    /// <param name="specialOffersFilterDto">The filter criteria for special offers.</param>  
    /// <param name="sortDto">The sorting criteria.</param>  
    /// <param name="perPage">The number of items per page.</param>  
    /// <param name="page">The page number.</param>  
    /// <returns>A paginated list of special offer DTOs.</returns>  
    Task<Pagination<SpecialOfferDto>> GetWithPagination(SpecialOfferFilterDto specialOffersFilterDto, SortDto sortDto, int perPage, int page);

    /// <summary>  
    /// Retrieves all special offers.  
    /// </summary>  
    /// <returns>A list of special offer DTOs.</returns>  
    Task<List<SpecialOfferDto>> GetAll();

    /// <summary>  
    /// Retrieves a special offer by its ID.  
    /// </summary>  
    /// <param name="id">The ID of the special offer.</param>  
    /// <returns>The special offer DTO, or null if not found.</returns>  
    Task<SpecialOfferDto?> GetById(long id);

    /// <summary>  
    /// Inserts a new special offer.  
    /// </summary>  
    /// <param name="specialOfferDto">The special offer DTO to insert.</param>  
    /// <returns>The inserted special offer DTO, or null if the insertion failed.</returns>  
    Task<SpecialOfferDto?> Insert(SpecialOfferDto specialOfferDto);

    /// <summary>  
    /// Updates a special offer by its ID.  
    /// </summary>  
    /// <param name="specialOfferDto">The special offer DTO to update.</param>  
    /// <returns>The updated special offer DTO, or null if the update failed.</returns>  
    Task<SpecialOfferDto?> UpdateById(SpecialOfferDto specialOfferDto);

    /// <summary>  
    /// Deletes a special offer by its ID.  
    /// </summary>  
    /// <param name="id">The ID of the special offer to delete.</param>  
    /// <returns>The deleted special offer DTO, or null if the deletion failed.</returns>  
    Task<SpecialOfferDto?> DeleteById(long id);

    /// <summary>  
    /// Deletes multiple special offers by their IDs.  
    /// </summary>  
    /// <param name="ids">The list of IDs of the special offers to delete.</param>  
    /// <returns>A list of deleted special offer DTOs.</returns>  
    Task<List<SpecialOfferDto>> DeleteByIds(List<long> ids);
}
