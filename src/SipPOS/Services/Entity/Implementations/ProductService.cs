using AutoMapper;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.Services.General.Interfaces;

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
    public async Task<List<ProductDto>> GetAll()
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        return mapper.Map<List<ProductDto>>(await productDao.GetAllAsync(storeId));
    }

    /// <summary>
    /// Gets a product by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product.</param>
    /// <returns>The product DTO if found; otherwise, null.</returns>
    public async Task<ProductDto?> GetById(long id)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        return mapper.Map<ProductDto>(await productDao.GetByIdAsync(storeId, id));
    }

    /// <summary>
    /// Inserts a new product.
    /// </summary>
    /// <param name="product">The product DTO to insert.</param>
    /// <returns>The inserted product DTO.</returns>
    public async Task<ProductDto?> Insert(ProductDto productDto)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();

        productDto.CreatedBy = staffAuthenticationService.Context.CurrentStaff?.CompositeUsername;
        productDto.CreatedAt = DateTime.Now;

        return mapper.Map<ProductDto>(await productDao.InsertAsync(storeId, mapper.Map<Product>(productDto)));
    }

    /// <summary>
    /// Updates a product by its identifier.
    /// </summary>
    /// <param name="product">The product DTO to update.</param>
    /// <returns>The updated product DTO.</returns>
    public async Task<ProductDto?> UpdateById(ProductDto productDto)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();

        productDto.UpdatedBy = staffAuthenticationService.Context.CurrentStaff?.CompositeUsername;
        productDto.UpdatedAt = DateTime.Now;

        return mapper.Map<ProductDto>(await productDao.UpdateByIdAsync(storeId, mapper.Map<Product>(productDto)));
    }

    /// <summary>
    /// Deletes a product by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the product to delete.</param>
    /// <returns>The deleted product DTO.</returns>
    public async Task<ProductDto?> DeleteById(long id)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        Staff author = staffAuthenticationService.Context.CurrentStaff ?? new Staff(1, new StaffDto());

        return mapper.Map<ProductDto>(await productDao.DeleteByIdAsync(storeId, id, author));
    }

    /// <summary>
    /// Deletes multiple products by their identifiers.
    /// </summary>
    /// <param name="ids">The identifiers of the products to delete.</param>
    /// <returns>A list of deleted product DTOs.</returns>
    public async Task<List<ProductDto>> DeleteByIds(List<long> ids)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        StaffAuthenticationService staffAuthenticationService = (StaffAuthenticationService)App.GetService<IStaffAuthenticationService>();
        Staff author = staffAuthenticationService.Context.CurrentStaff ?? new Staff(1, new StaffDto());
        
        return mapper.Map<List<ProductDto>>(await productDao.DeleteByIdsAsync(storeId, ids, author));
    }

    /// <summary>
    /// Searches for products based on filters and sorts.
    /// </summary>
    /// <param name="productFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorts to apply.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <param name="page">The page number.</param>
    /// <returns>A paginated list of product DTOs.</returns>
    public async Task<Pagination<ProductDto>> GetWithPagination(ProductFilterDto productFilterDto, SortDto sortDto, int perPage, int page)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();
        var storeId = storeAuthenticationService.GetCurrentStoreId();

        return mapper.Map<Pagination<ProductDto>>(await productDao.GetWithPaginationAsync(storeId, productFilterDto, sortDto, perPage, page));
    }
}
