using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Models.General;

namespace SipPOS.Services.Entity.Interfaces;

/// <summary>
/// Service interface for managing categories.
/// </summary>
public interface ICategoryService
{
    /// <summary>
    /// Searches for categories based on filters and sorts.
    /// </summary>
    /// <param name="categoryFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorts to apply.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <param name="page">The page number.</param>
    /// <returns>A pagination object containing the search results.</returns>
    Task<Pagination<CategoryDto>> GetWithPagination(CategoryFilterDto categoryFilterDto, SortDto sortDto, int perPage, int page);

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>A list of all category DTOs.</returns>
    Task<List<CategoryDto>> GetAll();

    /// <summary>
    /// Gets a category by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the category.</param>
    /// <returns>The category DTO if found; otherwise, null.</returns>
    Task<CategoryDto?> GetById(long id);

    /// <summary>
    /// Deletes a category by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the category to delete.</param>
    /// <returns>The deleted category DTO if successful; otherwise, null.</returns>
    Task<CategoryDto?> DeleteById(long id);

    /// <summary>
    /// Deletes multiple categories by their identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the categories to delete.</param>
    /// <returns>A list of deleted category DTOs.</returns>
    Task<List<CategoryDto>> DeleteByIds(List<long> ids);

    /// <summary>
    /// Inserts a new category.
    /// </summary>
    /// <param name="categoryDto">The category DTO to insert.</param>
    /// <returns>The inserted category DTO if successful; otherwise, null.</returns>
    Task<CategoryDto?> Insert(CategoryDto categoryDto);

    /// <summary>
    /// Updates a category by its identifier.
    /// </summary>
    /// <param name="categoryDto">The category DTO to update.</param>
    /// <returns>The updated category DTO if successful; otherwise, null.</returns>
    Task<CategoryDto?> UpdateById(CategoryDto categoryDto);
}
