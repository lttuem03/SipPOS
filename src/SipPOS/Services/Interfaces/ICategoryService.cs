using System.Collections.Generic;
using System.Threading.Tasks;

using SipPOS.Models;
using SipPOS.DataTransfer;
using SipPOS.DataAccess.Interfaces;

namespace SipPOS.Services.Interfaces;

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
    Pagination<CategoryDto> Search(CategoryFilterDto categoryFilterDto, SortDto sortDto, int perPage, int page);

    /// <summary>
    /// Gets all categories.
    /// </summary>
    /// <returns>A list of all category DTOs.</returns>
    IList<CategoryDto> GetAll();

    /// <summary>
    /// Gets a category by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the category.</param>
    /// <returns>The category DTO if found; otherwise, null.</returns>
    CategoryDto? GetById(long id);

    /// <summary>
    /// Deletes a category by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the category to delete.</param>
    /// <returns>The deleted category DTO if successful; otherwise, null.</returns>
    CategoryDto? DeleteById(long id);

    /// <summary>
    /// Deletes multiple categories by their identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the categories to delete.</param>
    /// <returns>A list of deleted category DTOs.</returns>
    IList<CategoryDto> DeleteByIds(IList<long> ids);

    /// <summary>
    /// Inserts a new category.
    /// </summary>
    /// <param name="categoryDto">The category DTO to insert.</param>
    /// <returns>The inserted category DTO if successful; otherwise, null.</returns>
    CategoryDto? Insert(CategoryDto categoryDto);

    /// <summary>
    /// Updates a category by its identifier.
    /// </summary>
    /// <param name="categoryDto">The category DTO to update.</param>
    /// <returns>The updated category DTO if successful; otherwise, null.</returns>
    CategoryDto? UpdateById(CategoryDto categoryDto);
}
