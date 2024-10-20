using SipPOS.DataAccess.Implementations;
using SipPOS.Models;
using SipPOS.Services;
using SipPOS.DataAccess.Interfaces;

namespace SipPOS.Services.Impl;

public class ProductService : IProductService
{
   
    private IProductDao _productDao;


    public ProductService(IProductDao productDao)
    {
        _productDao = productDao;
    }

    public IEnumerable<Product> GetAll()
    {
        return _productDao.GetAll();
    }

    public Product GetById(int id)
    {
        return _productDao.GetById(id);
    }

    public Product Insert(Product product)
    {
        return _productDao.Insert(product);
    }

    public Product UpdateById(Product product)
    {
        return _productDao.UpdateById(product);
    }

    public Product DeleteById(int id)
    {
        return _productDao.DeleteById(id);
    }

}
