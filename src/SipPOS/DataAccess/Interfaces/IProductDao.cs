using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SipPOS.Models;

namespace SipPOS.DataAccess.Interfaces;

/// <summary>
/// Data Access Object interface for Product.
/// </summary>
public interface IProductDao
{
    /// <summary>
    /// Searches for products based on filters and sorts, with pagination.
    /// </summary>
    /// <param name="filters">The filters to apply.</param>
    /// <param name="sorts">The sorts to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <returns>A paginated list of products.</returns>
    Pagination<Product> Search(IList<object> filters, IList<object> sorts, int page, int perPage);

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>A list of all products.</returns>
    IList<Product> GetAll();

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    Product? GetById(long id);

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>The deleted product if successful; otherwise, null.</returns>
    Product? DeleteById(long id);

    /// <summary>
    /// Deletes multiple products by their IDs.
    /// </summary>
    /// <param name="ids">The IDs of the products to delete.</param>
    /// <returns>A list of deleted products.</returns>
    IList<Product> DeleteByIds(IList<long> ids);

    /// <summary>
    /// Updates a product by its ID.
    /// </summary>
    /// <param name="product">The product to update.</param>
    /// <returns>The updated product if successful; otherwise, null.</returns>
    Product? UpdateById(Product product);

    /// <summary>
    /// Inserts a new product.
    /// </summary>
    /// <param name="product">The product to insert.</param>
    /// <returns>The inserted product if successful; otherwise, null.</returns>
    Product? Insert(Product product);

    /// <summary>
    /// Counts the total number of products.
    /// </summary>
    /// <returns>The total number of products.</returns>
    long Count();
}
