using AutoMapper;

using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;

namespace SipPOS.Services.Entity.Implementations;

/// <summary>
/// Service implementation for managing categories.
/// </summary>
public class CategoryService : ICategoryService
{
    private readonly ICategoryDao categoryDao;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryService"/> class.
    /// </summary>
    /// <param name="categoryDao">The data access object for categories.</param>
    /// <param name="mapper">The mapper for converting between models and DTOs.</param>
    public CategoryService(ICategoryDao categoryDao, IMapper mapper)
    {
        this.categoryDao = categoryDao;
        this.mapper = mapper;
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>A list of category DTOs.</returns>
    public IList<CategoryDto> GetAll()
    {
        return mapper.Map<IList<CategoryDto>>(categoryDao.GetAll());
    }

    /// <summary>
    /// Gets a category by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the category.</param>
    /// <returns>The category DTO if found; otherwise, null.</returns>
    public CategoryDto? GetById(long id)
    {
        return mapper.Map<CategoryDto>(categoryDao.GetById(id));
    }

    /// <summary>
    /// Inserts a new category.
    /// </summary>
    /// <param name="categoryDto">The category DTO to insert.</param>
    /// <returns>The inserted category DTO.</returns>
    public CategoryDto? Insert(CategoryDto categoryDto)
    {
        return mapper.Map<CategoryDto>(categoryDao.Insert(mapper.Map<Category>(categoryDto)));
    }

    /// <summary>
    /// Deletes a category by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the category to delete.</param>
    /// <returns>The deleted category DTO.</returns>
    public CategoryDto? DeleteById(long id)
    {
        return mapper.Map<CategoryDto>(categoryDao.DeleteById(id));
    }

    /// <summary>
    /// Deletes multiple categories by their identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the categories to delete.</param>
    /// <returns>A list of deleted category DTOs.</returns>
    public IList<CategoryDto> DeleteByIds(IList<long> ids)
    {
        return mapper.Map<IList<CategoryDto>>(categoryDao.DeleteByIds(ids));
    }

    /// <summary>
    /// Searches for categories based on filters and sorts.
    /// </summary>
    /// <param name="categoryFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorts to apply.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <param name="page">The page number.</param>
    /// <returns>A paginated list of category DTOs.</returns>
    public Pagination<CategoryDto> Search(CategoryFilterDto categoryFilterDto, SortDto sortDto, int perPage, int page)
    {
        return mapper.Map<Pagination<CategoryDto>>(categoryDao.Search(categoryFilterDto, sortDto, perPage, page));
    }

    /// <summary>
    /// Updates a category by its identifier.
    /// </summary>
    /// <param name="categoryDto">The category DTO to update.</param>
    /// <returns>The updated category DTO.</returns>
    public CategoryDto? UpdateById(CategoryDto categoryDto)
    {
        return mapper.Map<CategoryDto>(categoryDao.UpdateById(mapper.Map<Category>(categoryDto)));
    }
}
