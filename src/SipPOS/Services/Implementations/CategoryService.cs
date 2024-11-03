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

    public IList<CategoryDto> GetAll()
    {
        return mapper.Map<IList<CategoryDto>>(categoryDao.GetAll());
    }

    public CategoryDto? GetById(long id)
    {
        return mapper.Map<CategoryDto>(categoryDao.GetById(id));
    }

    public CategoryDto? Insert(CategoryDto productDto)
    {
        return mapper.Map<CategoryDto>(categoryDao.Insert(mapper.Map<Category>(productDto)));
    }

    public CategoryDto? DeleteById(long id)
    {
        return mapper.Map<CategoryDto>(categoryDao.DeleteById(id));
    }

    public IList<CategoryDto> DeleteByIds(IList<long> ids)
    {
        return mapper.Map<IList<CategoryDto>>(categoryDao.DeleteByIds(ids));
    }

    public Pagination<CategoryDto> Search(CategoryFilterDto categoryFilterDto, SortDto sortDto, int perPage, int page)
    {
        return mapper.Map<Pagination<CategoryDto>>(categoryDao.Search(categoryFilterDto, sortDto, perPage, page));
    }

    public CategoryDto? UpdateById(CategoryDto productDto)
    {
        return mapper.Map<CategoryDto>(categoryDao.UpdateById(mapper.Map<Category>(productDto)));
    }
}
