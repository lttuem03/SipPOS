using SipPOS.DataTransfer.General;
using SipPOS.Models.Entity;
using SipPOS.Models.General;

namespace SipPOS.Services.DataAccess.Interfaces;

/// <summary>
/// Data Access Object interface for Product.
/// </summary>
public interface IProductDao
{
    /// <summary>
    /// Searches for products based on filters and sorts, with pagination.
    /// </summary>
    /// <param name="productFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorts to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <returns>A paginated list of products.</returns>
    Task<Pagination<Product>> SearchAsync(ProductFilterDto productFilterDto, SortDto sortDto, int page, int perPage);

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>A list of all products.</returns>
    Task<List<Product>> GetAllAsync();

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    Task<Product?> GetByIdAsync(long id);

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>The deleted product if successful; otherwise, null.</returns>
    Task<Product?> DeleteByIdAsync(long id, Staff author);

    /// <summary>
    /// Deletes multiple products by their IDs.
    /// </summary>
    /// <param name="ids">The IDs of the products to delete.</param>
    /// <returns>A list of deleted products.</returns>
    Task<List<Product>> DeleteByIdsAsync(List<long> ids, Staff author);

    /// <summary>
    /// Updates a product by its ID.
    /// </summary>
    /// <param name="product">The product to update.</param>
    /// <returns>The updated product if successful; otherwise, null.</returns>
    Task<Product?> UpdateByIdAsync(Product productDto);

    /// <summary>
    /// Inserts a new product.
    /// </summary>
    /// <param name="product">The product to insert.</param>
    /// <returns>The inserted product if successful; otherwise, null.</returns>
    Task<Product?> InsertAsync(Product productDto);

    /// <summary>
    /// Counts the total number of products.
    /// </summary>
    /// <returns>The total number of products.</returns>
    Task<long> CountAsync(ProductFilterDto productFilterDto);
}
