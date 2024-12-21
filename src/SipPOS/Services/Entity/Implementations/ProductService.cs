using AutoMapper;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Models.Entity;
using SipPOS.Models.General;

namespace SipPOS.Services.Entity.Implementations;

/// <summary>
/// Service implementation for managing products.
/// </summary>
public class ProductService : IProductService
{
    private readonly IProductDao productDao;
    private readonly IMapper mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="productDao">The data access object for products.</param>
    /// <param name="mapper">The mapper for converting between models and DTOs.</param>
    public ProductService(IProductDao productDao, IMapper mapper)
    {
        this.productDao = productDao;
        this.mapper = mapper;
    }

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>A list of product DTOs.</returns>
    public IList<ProductDto> GetAll()
    {
        return mapper.Map<IList<ProductDto>>(productDao.GetAll());
    }

    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product.</param>
    /// <returns>The product DTO if found; otherwise, null.</returns>
    public ProductDto? GetById(long id)
    {
        return mapper.Map<ProductDto>(productDao.GetById(id));
    }

    /// <summary>
    /// Inserts a new product.
    /// </summary>
    /// <param name="product">The product DTO to insert.</param>
    /// <returns>The inserted product DTO.</returns>
    public ProductDto? Insert(ProductDto product)
    {
        return mapper.Map<ProductDto>(productDao.Insert(mapper.Map<Product>(product)));
    }

    /// <summary>
    /// Updates a product by its identifier.
    /// </summary>
    /// <param name="product">The product DTO to update.</param>
    /// <returns>The updated product DTO.</returns>
    public ProductDto? UpdateById(ProductDto product)
    {
        return mapper.Map<ProductDto>(productDao.UpdateById(mapper.Map<Product>(product)));
    }

    /// <summary>
    /// Deletes a product by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product to delete.</param>
    /// <returns>The deleted product DTO.</returns>
    public ProductDto? DeleteById(long id)
    {
        return mapper.Map<ProductDto>(productDao.DeleteById(id));
    }

    /// <summary>
    /// Deletes multiple products by their identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the products to delete.</param>
    /// <returns>A list of deleted product DTOs.</returns>
    public IList<ProductDto> DeleteByIds(IList<long> ids)
    {
        return mapper.Map<IList<ProductDto>>(productDao.DeleteByIds(ids));
    }

    /// <summary>
    /// Searches for products based on filters and sorts.
    /// </summary>
    /// <param name="productFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorts to apply.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <param name="page">The page number.</param>
    /// <returns>A paginated list of product DTOs.</returns>
    public Pagination<ProductDto> Search(ProductFilterDto productFilterDto, SortDto sortDto, int perPage, int page)
    {
        return mapper.Map<Pagination<ProductDto>>(productDao.Search(productFilterDto, sortDto, perPage, page));
    }
}
