using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Models.General;

namespace SipPOS.Services.Entity.Interfaces;

/// <summary>
/// Service interface for managing products.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Searches for products based on filters and sorts.
    /// </summary>
    /// <param name="productFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorts to apply.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <param name="page">The page number.</param>
    /// <returns>A pagination object containing the search results.</returns>
    Pagination<ProductDto> Search(ProductFilterDto productFilterDto, SortDto sortDto, int perPage, int page);

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>A list of all product DTOs.</returns>
    List<ProductDto> GetAll();

    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product.</param>
    /// <returns>The product DTO if found; otherwise, null.</returns>
    ProductDto? GetById(long id);

    /// <summary>
    /// Inserts a new product.
    /// </summary>
    /// <param name="productDto">The product DTO to insert.</param>
    /// <returns>The inserted product DTO if successful; otherwise, null.</returns>
    ProductDto? Insert(ProductDto productDto);

    /// <summary>
    /// Updates a product by its identifier.
    /// </summary>
    /// <param name="productDto">The product DTO to update.</param>
    /// <returns>The updated product DTO if successful; otherwise, null.</returns>
    ProductDto? UpdateById(ProductDto productDto);

    /// <summary>
    /// Deletes a product by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product to delete.</param>
    /// <returns>The deleted product DTO if successful; otherwise, null.</returns>
    ProductDto? DeleteById(long id);

    /// <summary>
    /// Deletes multiple products by their identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the products to delete.</param>
    /// <returns>A list of deleted product DTOs.</returns>
    List<ProductDto> DeleteByIds(List<long> ids);
}
