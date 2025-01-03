using SipPOS.DataTransfer.General;
using SipPOS.Models.Entity;
using SipPOS.Models.General;

namespace SipPOS.Services.DataAccess.Interfaces;

public interface ISpecialOfferDao
{
    Task<SpecialOffer?> InsertAsync(long storeId, SpecialOffer specialOffers);

    Task<List<SpecialOffer>> GetAllAsync(long storeId);

    Task<SpecialOffer?> GetByIdAsync(long storeId, long id);

    Task<Pagination<SpecialOffer>> GetWithPaginationAsync
    (
        long storeId,
        SpecialOfferFilterDto specialOffersFilterDto, 
        SortDto sortDto, 
        int page, 
        int perPage
    );

    Task<SpecialOffer?> UpdateByIdAsync(long storeId, SpecialOffer specialOffers);

    Task<SpecialOffer?> DeleteByIdAsync(long storeId, long id, Staff author);

    Task<List<SpecialOffer>> DeleteByIdsAsync(long storeId, List<long> ids, Staff author);

    Task<long> CountAsync(long storeId, SpecialOfferFilterDto specialOffersFilterDto);
}
