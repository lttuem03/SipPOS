using SipPOS.Models;
using SipPOS.Services;
using SipPOS.DataAccess.Interfaces;
using AutoMapper;
using SipPOS.DataTransfer;

namespace SipPOS.Services.Impl;

public class CategoryService : ICategoryService
{
    private readonly ICategoryDao categoryDao;
    private readonly IMapper mapper;

    public CategoryService(ICategoryDao categoryDao, IMapper mapper)
    {
        this.categoryDao = categoryDao;
        this.mapper = mapper;
    }

    public IEnumerable<CategoryDto> GetAll()
    {
        return mapper.Map<IEnumerable<CategoryDto>>(categoryDao.GetAll());
    }

    public CategoryDto GetById(int id)
    {
        return mapper.Map<CategoryDto>(categoryDao.GetById(id));
    }

    public CategoryDto Insert(CategoryDto productDto)
    {
        return mapper.Map<CategoryDto>(categoryDao.Insert(mapper.Map<Category>(productDto)));
    }

    public CategoryDto UpdateById(CategoryDto productDto)
    {
        return mapper.Map<CategoryDto>(categoryDao.UpdateById(mapper.Map<Category>(productDto)));
    }
}
