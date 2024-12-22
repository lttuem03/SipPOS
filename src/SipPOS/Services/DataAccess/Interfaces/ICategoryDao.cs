using SipPOS.DataTransfer.General;
using SipPOS.Models.Entity;
using SipPOS.Models.General;

namespace SipPOS.Services.DataAccess.Interfaces;

/// <summary>
/// Data Access Object interface for Category.
/// </summary>
public interface ICategoryDao
{
    /// <summary>
    /// Searches for categories based on filters and sorts, with pagination.
    /// </summary>
    /// <param name="categoryFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorts to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <returns>A paginated list of categories.</returns>
    Task<Pagination<Category>> SearchAsync(CategoryFilterDto categoryFilterDto, SortDto sortDto, int page, int perPage);

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <returns>A list of all categories.</returns>
    Task<List<Category>> GetAllAsync();

    /// <summary>
    /// Retrieves a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to retrieve.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    Task<Category?> GetByIdAsync(long id);

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>The deleted category if successful; otherwise, null.</returns>
    Task<Category?> DeleteByIdAsync(long id, Staff author);

    /// <summary>
    /// Deletes multiple categories by their IDs.
    /// </summary>
    /// <param name="ids">The IDs of the categories to delete.</param>
    /// <returns>A list of deleted categories.</returns>
    Task<List<Category>> DeleteByIdsAsync(List<long> ids, Staff author);

    /// <summary>
    /// Updates a category by its ID.
    /// </summary>
    /// <param name="category">The category to update.</param>
    /// <returns>The updated category if successful; otherwise, null.</returns>
    Task<Category?> UpdateByIdAsync(Category categoryDto);

    /// <summary>
    /// Inserts a new category.
    /// </summary>
    /// <param name="category">The category to insert.</param>
    /// <returns>The inserted category if successful; otherwise, null.</returns>
    Task<Category?> InsertAsync(Category categoryDto);

    /// <summary>
    /// Counts the total number of categories.
    /// </summary>
    /// <returns>The total number of categories.</returns>
    Task<long> CountAsync(CategoryFilterDto categoryFilterDto);
}
