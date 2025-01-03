using AutoMapper;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.Services.General.Interfaces;

namespace SipPOS.Services.Entity.Implementations;

public class SpecialOfferService : ISpecialOfferService
{
    private readonly ISpecialOfferDao specialOffersDao;
    private readonly IMapper mapper;

    public SpecialOfferService(ISpecialOfferDao specialOffersDao, IMapper mapper)
    {
        this.specialOffersDao = specialOffersDao;
        this.mapper = mapper;
    }

    public async Task<List<SpecialOfferDto>> GetAll()
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        return mapper.Map<List<SpecialOfferDto>>(await specialOffersDao.GetAllAsync(storeId));
    }

    public async Task<SpecialOfferDto?> GetById(long id)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        return mapper.Map<SpecialOfferDto>(await specialOffersDao.GetByIdAsync(storeId, id));
    }

    public async Task<SpecialOfferDto?> Insert(SpecialOfferDto specialOfferDto)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();

        specialOfferDto.CreatedBy = staffAuthenticationService.Context.CurrentStaff?.CompositeUsername;
        specialOfferDto.CreatedAt = DateTime.Now;

        return mapper.Map<SpecialOfferDto>(await specialOffersDao.InsertAsync(storeId, mapper.Map<SpecialOffer>(specialOfferDto)));
    }

    public async Task<SpecialOfferDto?> UpdateById(SpecialOfferDto specialOfferDto)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();

        specialOfferDto.UpdatedBy = staffAuthenticationService.Context.CurrentStaff?.CompositeUsername;
        specialOfferDto.UpdatedAt = DateTime.Now;

        return mapper.Map<SpecialOfferDto>(await specialOffersDao.UpdateByIdAsync(storeId, mapper.Map<SpecialOffer>(specialOfferDto)));
    }

    public async Task<SpecialOfferDto?> DeleteById(long id)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        Staff author = staffAuthenticationService.Context.CurrentStaff ?? new Staff(1, new StaffDto());

        return mapper.Map<SpecialOfferDto>(await specialOffersDao.DeleteByIdAsync(storeId, id, author));
    }

    public async Task<List<SpecialOfferDto>> DeleteByIds(List<long> ids)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        Staff author = staffAuthenticationService.Context.CurrentStaff ?? new Staff(1, new StaffDto());
        
        return mapper.Map<List<SpecialOfferDto>>(await specialOffersDao.DeleteByIdsAsync(storeId, ids, author));
    }

    public async Task<Pagination<SpecialOfferDto>> GetWithPagination(SpecialOfferFilterDto specialOffersFilterDto, SortDto sortDto, int perPage, int page)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        return mapper.Map<Pagination<SpecialOfferDto>>(await specialOffersDao.GetWithPaginationAsync(storeId, specialOffersFilterDto, sortDto, perPage, page));
    }
}
