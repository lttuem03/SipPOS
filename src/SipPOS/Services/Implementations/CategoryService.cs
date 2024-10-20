using SipPOS.Models;
using SipPOS.Services;
using SipPOS.DataAccess.Interfaces;

namespace SipPOS.Services.Impl;

public class CategoryService : ICategoryService
{
    private readonly ICategoryDao _categoryDao;

    public CategoryService(ICategoryDao categoryDao)
    {
        _categoryDao = categoryDao;
    }

    public IEnumerable<Category> GetAll()
    {
        return _categoryDao.GetAll();
    }

    public Category GetById(int id)
    {
        return _categoryDao.GetById(id);
    }

    public Category Insert(Category product)
    {
        return _categoryDao.Insert(product);
    }

    public Category UpdateById(Category product)
    {
        return _categoryDao.UpdateById(product);
    }
}
