using AutoMapper;

using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.Authentication.Interfaces;

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
    public List<CategoryDto> GetAll()
    {
        return mapper.Map<List<CategoryDto>>(categoryDao.GetAllAsync().Result);
    }

    /// <summary>
    /// Gets a category by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the category.</param>
    /// <returns>The category DTO if found; otherwise, null.</returns>
    public CategoryDto? GetById(long id)
    {
        return mapper.Map<CategoryDto>(categoryDao.GetByIdAsync(id).Result);
    }

    /// <summary>
    /// Inserts a new category.
    /// </summary>
    /// <param name="categoryDto">The category DTO to insert.</param>
    /// <returns>The inserted category DTO.</returns>
    public CategoryDto? Insert(CategoryDto categoryDto)
    {
        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        categoryDto.CreatedBy = staffAuthenticationService.Context.CurrentStaff?.CompositeUsername;
        categoryDto.CreatedAt = DateTime.Now;
        return mapper.Map<CategoryDto>(categoryDao.InsertAsync(mapper.Map<Category>(categoryDto)).Result);
    }

    /// <summary>
    /// Deletes a category by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the category to delete.</param>
    /// <returns>The deleted category DTO.</returns>
    public CategoryDto? DeleteById(long id)
    {
        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        Staff author = staffAuthenticationService.Context.CurrentStaff ?? new Staff(1, new StaffDto());
        return mapper.Map<CategoryDto>(categoryDao.DeleteByIdAsync(id, author).Result);
    }

    /// <summary>
    /// Deletes multiple categories by their identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the categories to delete.</param>
    /// <returns>A list of deleted category DTOs.</returns>
    public List<CategoryDto> DeleteByIds(List<long> ids)
    {
        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        Staff author = staffAuthenticationService.Context.CurrentStaff ?? new Staff(1, new StaffDto());
        return mapper.Map<List<CategoryDto>>(categoryDao.DeleteByIdsAsync(ids, author).Result);
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
        return mapper.Map<Pagination<CategoryDto>>(categoryDao.SearchAsync(categoryFilterDto, sortDto, perPage, page).Result);
    }

    /// <summary>
    /// Updates a category by its identifier.
    /// </summary>
    /// <param name="categoryDto">The category DTO to update.</param>
    /// <returns>The updated category DTO.</returns>
    public CategoryDto? UpdateById(CategoryDto categoryDto)
    {
        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        categoryDto.UpdatedBy = staffAuthenticationService.Context.CurrentStaff?.CompositeUsername;
        categoryDto.UpdatedAt = DateTime.Now;
        return mapper.Map<CategoryDto>(categoryDao.UpdateByIdAsync(mapper.Map<Category>(categoryDto)).Result);
    }
}
