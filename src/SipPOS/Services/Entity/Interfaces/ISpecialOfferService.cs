using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Models.General;
using SipPOS.Services.DataAccess.Interfaces;

namespace SipPOS.Services.Entity.Interfaces;

public interface ISpecialOfferService
{

    Task<Pagination<SpecialOfferDto>> GetWithPagination(SpecialOfferFilterDto specialOffersFilterDto, SortDto sortDto, int perPage, int page);

    Task<List<SpecialOfferDto>> GetAll();

    Task<SpecialOfferDto?> GetById(long id);

    Task<SpecialOfferDto?> Insert(SpecialOfferDto specialOfferDto);

    Task<SpecialOfferDto?> UpdateById(SpecialOfferDto specialOfferDto);

    Task<SpecialOfferDto?> DeleteById(long id);

    Task<List<SpecialOfferDto>> DeleteByIds(List<long> ids);
}
