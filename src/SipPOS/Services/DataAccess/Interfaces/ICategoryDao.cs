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
    /// Inserts a new category into a store.
    /// </summary>
    /// <param name="storeId">The id of the store to insert category into.</param>
    /// <param name="category">The category to insert.</param>
    /// <returns>The inserted category if successful; otherwise, null.</returns>
    Task<Category?> InsertAsync(long storeId, Category category);

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <param name="storeId">The id of the store to get categories from.</param>
    /// <returns>A list of all categories.</returns>
    Task<List<Category>> GetAllAsync(long storeId);

    /// <summary>
    /// Retrieves a category by its ID.
    /// </summary>
    /// <param name="storeId">The id of the store to get categories from.</param>
    /// <param name="id">The ID of the category to retrieve.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    Task<Category?> GetByIdAsync(long storeId, long id);

    /// <summary>
    /// Searches for categories based on filters and sorts, with pagination.
    /// </summary>
    /// <param name="storeId">The id of the store to search.</param>
    /// <param name="categoryFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorts to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <returns>A paginated list of categories.</returns>
    Task<Pagination<Category>> GetWithPaginationAsync
    (
        long storeId, 
        CategoryFilterDto categoryFilterDto, 
        SortDto sortDto, 
        int page, 
        int perPage
    );

    /// <summary>
    /// Updates a category by its ID.
    /// </summary>
    /// <param name="storeId">The id of the store to update category from.</param>
    /// <param name="category">The category to update.</param>
    /// <returns>The updated category if successful; otherwise, null.</returns>
    Task<Category?> UpdateByIdAsync(long storeId, Category category);

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="storeId">The id of the store to delete categories from.</param>
    /// <param name="id">The ID of the category to delete.</param>
    /// <param name="author">The author of the operation.</param>
    /// <returns>The deleted category if successful; otherwise, null.</returns>
    Task<Category?> DeleteByIdAsync(long storeId, long id, Staff author);

    /// <summary>
    /// Deletes multiple categories by their IDs.
    /// </summary>
    /// <param name="storeId">The id of the store to delete categories from.</param>
    /// <param name="ids">The IDs of the categories to delete.</param>
    /// <param name="author">The author of the operation.</param>
    /// <returns>A list of deleted categories.</returns>
    Task<List<Category>> DeleteByIdsAsync(long storeId, List<long> ids, Staff author);

    /// <summary>
    /// Counts the total number of categories that fit the filters.
    /// </summary>
    /// <param name="storeId">The id of the store to count in.</param>
    /// <param name="categoryFilterDto">The filter to apply to the count query.</param>
    /// <returns>The total number of categories that fit the filters.</returns>
    Task<long> CountAsync(long storeId, CategoryFilterDto categoryFilterDto);
}
