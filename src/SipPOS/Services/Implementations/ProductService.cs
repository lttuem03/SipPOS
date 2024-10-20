using SipPOS.DataAccess.Implementations;
using SipPOS.Models;
using SipPOS.Services;
using SipPOS.DataAccess.Interfaces;
using AutoMapper;
using SipPOS.DataTransfer;

namespace SipPOS.Services.Impl;

public class ProductService : IProductService
{

    private IProductDao productDao;
    private readonly IMapper mapper;


    public ProductService(IProductDao productDao, IMapper mapper)
    {
        this.productDao = productDao;
        this.mapper = mapper;
    }

    public IEnumerable<ProductDto> GetAll()
    {
        return mapper.Map<IEnumerable<ProductDto>>(productDao.GetAll());
    }

    public ProductDto GetById(int id)
    {
        return mapper.Map<ProductDto>(productDao.GetById(id));
    }

    public ProductDto Insert(ProductDto product)
    {
        return mapper.Map<ProductDto>(productDao.Insert(mapper.Map<Product>(product)));
    }

    public ProductDto UpdateById(ProductDto product)
    {
        return mapper.Map<ProductDto>(productDao.UpdateById(mapper.Map<Product>(product)));
    }

    public ProductDto DeleteById(int id)
    {
        return mapper.Map<ProductDto>(productDao.DeleteById(id));
    }

}
