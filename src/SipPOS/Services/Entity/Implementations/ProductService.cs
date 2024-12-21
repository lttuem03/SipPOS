using AutoMapper;

using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.Authentication.Interfaces;

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
    public List<ProductDto> GetAll()
    {
        return mapper.Map<List<ProductDto>>(productDao.GetAllAsync().Result);
    }

    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product.</param>
    /// <returns>The product DTO if found; otherwise, null.</returns>
    public ProductDto? GetById(long id)
    {
        return mapper.Map<ProductDto>(productDao.GetByIdAsync(id).Result);
    }

    /// <summary>
    /// Inserts a new product.
    /// </summary>
    /// <param name="product">The product DTO to insert.</param>
    /// <returns>The inserted product DTO.</returns>
    public ProductDto? Insert(ProductDto productDto)
    {
        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        productDto.CreatedBy = staffAuthenticationService.Context.CurrentStaff?.CompositeUsername;
        productDto.CreatedAt = DateTime.Now;
        return mapper.Map<ProductDto>(productDao.InsertAsync(mapper.Map<Product>(productDto)).Result);
    }

    /// <summary>
    /// Updates a product by its identifier.
    /// </summary>
    /// <param name="product">The product DTO to update.</param>
    /// <returns>The updated product DTO.</returns>
    public ProductDto? UpdateById(ProductDto productDto)
    {
        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        productDto.UpdatedBy = staffAuthenticationService.Context.CurrentStaff?.CompositeUsername;
        productDto.UpdatedAt = DateTime.Now;
        return mapper.Map<ProductDto>(productDao.UpdateByIdAsync(mapper.Map<Product>(productDto)).Result);
    }

    /// <summary>
    /// Deletes a product by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product to delete.</param>
    /// <returns>The deleted product DTO.</returns>
    public ProductDto? DeleteById(long id)
    {
        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        Staff author = staffAuthenticationService.Context.CurrentStaff ?? new Staff(1, new StaffDto());
        return mapper.Map<ProductDto>(productDao.DeleteByIdAsync(id, author).Result);
    }

    /// <summary>
    /// Deletes multiple products by their identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the products to delete.</param>
    /// <returns>A list of deleted product DTOs.</returns>
    public List<ProductDto> DeleteByIds(List<long> ids)
    {
        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        Staff author = staffAuthenticationService.Context.CurrentStaff ?? new Staff(1, new StaffDto());
        return mapper.Map<List<ProductDto>>(productDao.DeleteByIdsAsync(ids, author).Result);
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
        return mapper.Map<Pagination<ProductDto>>(productDao.SearchAsync(productFilterDto, sortDto, perPage, page).Result);
    }
}
