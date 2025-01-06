using AutoMapper;

using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.General.Interfaces;

namespace SipPOS.Services.Entity.Implementations;

/// <summary>
/// Service for managing special offers.
/// </summary>
public class SpecialOfferService : ISpecialOfferService
{
    private readonly ISpecialOfferDao specialOffersDao;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecialOfferService"/> class.
    /// </summary>
    /// <param name="specialOffersDao">The data access object for special offers.</param>
    /// <param name="mapper">The AutoMapper instance.</param>
    public SpecialOfferService(ISpecialOfferDao specialOffersDao, IMapper mapper)
    {
        this.specialOffersDao = specialOffersDao;
        this.mapper = mapper;
    }

    /// <summary>
    /// Retrieves all special offers.
    /// </summary>
    /// <returns>A list of special offer DTOs.</returns>
    public async Task<List<SpecialOfferDto>> GetAll()
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        return mapper.Map<List<SpecialOfferDto>>(await specialOffersDao.GetAllAsync(storeId));
    }

    /// <summary>
    /// Retrieves a special offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the special offer.</param>
    /// <returns>The special offer DTO, or null if not found.</returns>
    public async Task<SpecialOfferDto?> GetById(long id)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        return mapper.Map<SpecialOfferDto>(await specialOffersDao.GetByIdAsync(storeId, id));
    }

    /// <summary>
    /// Inserts a new special offer.
    /// </summary>
    /// <param name="specialOfferDto">The special offer DTO to insert.</param>
    /// <returns>The inserted special offer DTO, or null if the insertion failed.</returns>
    public async Task<SpecialOfferDto?> Insert(SpecialOfferDto specialOfferDto)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();

        specialOfferDto.CreatedBy = staffAuthenticationService.Context.CurrentStaff?.CompositeUsername;
        specialOfferDto.CreatedAt = DateTime.Now;

        return mapper.Map<SpecialOfferDto>(await specialOffersDao.InsertAsync(storeId, mapper.Map<SpecialOffer>(specialOfferDto)));
    }

    /// <summary>
    /// Updates a special offer by its ID.
    /// </summary>
    /// <param name="specialOfferDto">The special offer DTO to update.</param>
    /// <returns>The updated special offer DTO, or null if the update failed.</returns>
    public async Task<SpecialOfferDto?> UpdateById(SpecialOfferDto specialOfferDto)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();

        specialOfferDto.UpdatedBy = staffAuthenticationService.Context.CurrentStaff?.CompositeUsername;
        specialOfferDto.UpdatedAt = DateTime.Now;

        return mapper.Map<SpecialOfferDto>(await specialOffersDao.UpdateByIdAsync(storeId, mapper.Map<SpecialOffer>(specialOfferDto)));
    }

    /// <summary>
    /// Deletes a special offer by its ID.
    /// </summary>
    /// <param name="id">The ID of the special offer to delete.</param>
    /// <returns>The deleted special offer DTO, or null if the deletion failed.</returns>
    public async Task<SpecialOfferDto?> DeleteById(long id)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        Staff author = staffAuthenticationService.Context.CurrentStaff ?? new Staff(1, new StaffDto());

        return mapper.Map<SpecialOfferDto>(await specialOffersDao.DeleteByIdAsync(storeId, id, author));
    }

    /// <summary>
    /// Deletes multiple special offers by their IDs.
    /// </summary>
    /// <param name="ids">The list of IDs of the special offers to delete.</param>
    /// <returns>A list of deleted special offer DTOs.</returns>
    public async Task<List<SpecialOfferDto>> DeleteByIds(List<long> ids)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        Staff author = staffAuthenticationService.Context.CurrentStaff ?? new Staff(1, new StaffDto());

        return mapper.Map<List<SpecialOfferDto>>(await specialOffersDao.DeleteByIdsAsync(storeId, ids, author));
    }

    /// <summary>
    /// Retrieves special offers with pagination and filtering.
    /// </summary>
    /// <param name="specialOffersFilterDto">The filter criteria for special offers.</param>
    /// <param name="sortDto">The sorting criteria.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <param name="page">The page number.</param>
    /// <returns>A paginated list of special offer DTOs.</returns>
    public async Task<Pagination<SpecialOfferDto>> GetWithPagination(SpecialOfferFilterDto specialOffersFilterDto, SortDto sortDto, int perPage, int page)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        return mapper.Map<Pagination<SpecialOfferDto>>(await specialOffersDao.GetWithPaginationAsync(storeId, specialOffersFilterDto, sortDto, perPage, page));
    }
}
