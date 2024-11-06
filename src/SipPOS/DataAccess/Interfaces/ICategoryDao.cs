using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;

namespace SipPOS.DataAccess.Interfaces;

/// <summary>
/// Data Access Object interface for Category.
/// </summary>
public interface ICategoryDao
{
    /// <summary>
    /// Searches for categories based on filters and sorts, with pagination.
    /// </summary>
    /// <param name="filters">The filters to apply.</param>
    /// <param name="sorts">The sorts to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <returns>A paginated list of categories.</returns>
    Pagination<Category> Search(IList<object> filters, IList<object> sorts, int page, int perPage);

    /// <summary>
    /// Retrieves all categories.
    /// </summary>
    /// <returns>A list of all categories.</returns>
    IList<Category> GetAll();

    /// <summary>
    /// Retrieves a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to retrieve.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    Category? GetById(long id);

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>The deleted category if successful; otherwise, null.</returns>
    Category? DeleteById(long id);

    /// <summary>
    /// Deletes multiple categories by their IDs.
    /// </summary>
    /// <param name="ids">The IDs of the categories to delete.</param>
    /// <returns>A list of deleted categories.</returns>
    IList<Category> DeleteByIds(IList<long> ids);

    /// <summary>
    /// Updates a category by its ID.
    /// </summary>
    /// <param name="category">The category to update.</param>
    /// <returns>The updated category if successful; otherwise, null.</returns>
    Category? UpdateById(Category category);

    /// <summary>
    /// Inserts a new category.
    /// </summary>
    /// <param name="category">The category to insert.</param>
    /// <returns>The inserted category if successful; otherwise, null.</returns>
    Category? Insert(Category category);

    /// <summary>
    /// Counts the total number of categories.
    /// </summary>
    /// <returns>The total number of categories.</returns>
    long Count();
}
