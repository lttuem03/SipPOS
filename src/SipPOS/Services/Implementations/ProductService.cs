using AutoMapper;

using SipPOS.Models;
using SipPOS.Services.Interfaces;
using SipPOS.DataAccess;
using SipPOS.DataAccess.Interfaces;
using SipPOS.DataAccess.Implementations;
using SipPOS.DataTransfer;

namespace SipPOS.Services.Implementations;

public class ProductService : IProductService
{

    private IProductDao productDao;

    private readonly IMapper mapper;

    public ProductService(IProductDao productDao, IMapper mapper)
    {
        this.productDao = productDao;
        this.mapper = mapper;
    }

    public IList<ProductDto> GetAll()
    {
        return mapper.Map<IList<ProductDto>>(productDao.GetAll());
    }

    public ProductDto? GetById(long id)
    {
        return mapper.Map<ProductDto>(productDao.GetById(id));
    }

    public ProductDto? Insert(ProductDto product)
    {
        return mapper.Map<ProductDto>(productDao.Insert(mapper.Map<Product>(product)));
    }

    public ProductDto? UpdateById(ProductDto product)
    {
        return mapper.Map<ProductDto>(productDao.UpdateById(mapper.Map<Product>(product)));
    }

    public ProductDto? DeleteById(long id)
    {
        return mapper.Map<ProductDto>(productDao.DeleteById(id));
    }

    public IList<ProductDto> DeleteByIds(IList<long> ids)
    {
        return mapper.Map<IList<ProductDto>>(productDao.DeleteByIds(ids));
    }

    public Pagination<ProductDto> Search(ProductFilterDto productFilterDto, SortDto sortDto, int perPage, int page)
    {
        return mapper.Map<Pagination<ProductDto>>(productDao.Search(productFilterDto, sortDto, perPage, page));
    }
}
