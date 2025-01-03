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
    /// Inserts a new product.
    /// </summary>
    /// <param name="storeId">The store ID of the product.</param>
    /// <param name="product">The product to insert.</param>
    /// <returns>The inserted product if successful; otherwise, null.</returns>
    Task<Product?> InsertAsync(long storeId, Product product);

    /// <summary>
    /// Retrieves all products of a store.
    /// </summary>
    /// <param name="storeId">The store id to get products from.</param>
    /// <returns>A list of all products.</returns>
    Task<List<Product>> GetAllAsync(long storeId);

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="storeId">The store ID of the store to retrieve from.</param>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    Task<Product?> GetByIdAsync(long storeId, long id);

    /// <summary>
    /// Searches for products based on filters and sorts, with pagination.
    /// </summary>
    /// <param name="storeId">The store ID of the store to retrieve from.</param>
    /// <param name="productFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorts to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <returns>A paginated list of products.</returns>
    Task<Pagination<Product>> GetWithPaginationAsync
    (
        long storeId, 
        ProductFilterDto productFilterDto, 
        SortDto sortDto, 
        int page, 
        int perPage
    );

    /// <summary>
    /// Updates a product by its ID.
    /// </summary>
    /// <param name="storeId">The store ID of the store to update product from.</param>
    /// <param name="product">The product to update.</param>
    /// <returns>The updated product if successful; otherwise, null.</returns>
    Task<Product?> UpdateByIdAsync(long storeId, Product product);

    /// <summary>
    /// "Soft" deletes a product by its ID.
    /// </summary>
    /// <param name="storeId">The store ID of the store to delete product from.</param>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>The deleted product if successful; otherwise, null.</returns>
    Task<Product?> DeleteByIdAsync(long storeId, long id, Staff author);

    /// <summary>
    /// "Soft" deletes multiple products by their IDs.
    /// </summary>
    /// <param name="storeId">The store ID of the store to delete products from.</param>
    /// <param name="ids">The IDs of the products to delete.</param>
    /// <param name="author">The author of the operation.</param>
    /// <returns>A list of deleted products.</returns>
    Task<List<Product>> DeleteByIdsAsync(long storeId, List<long> ids, Staff author);

    /// <summary>
    /// Counts the total number of products that the filter applies to.
    /// </summary>
    /// <param name="storeId">The store id of the store to count from.</param>
    /// <param name="productFilterDto">The filters to apply.</param>
    /// <returns>The total number of products.</returns>
    Task<long> CountAsync(long storeId, ProductFilterDto productFilterDto);
}
