using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using SipPOS.Models.General;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Services.Entity.Interfaces;

namespace SipPOS.ViewModels.Inventory;

/// <summary>
/// ViewModel for managing products, handling data for data-binding and the logic in the ProductManagementView.
/// </summary>
public partial class ProductManagementViewModel : ObservableRecipient
{
    /// <summary>
    /// Gets the collection of products.
    /// </summary>
    public ObservableCollection<ProductDto> Products { get; } = new ObservableCollection<ProductDto>();

    /// <summary>
    /// Gets the collection of categories.
    /// </summary>
    public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();

    /// <summary>
    /// Gets the collection of category filters.
    /// </summary>
    public ObservableCollection<CategoryDto> CategoriesFilter { get; } = new ObservableCollection<CategoryDto>();

    /// <summary>
    /// Gets the collection of status items.
    /// </summary>
    public ObservableCollection<StatusItem> StatusItems { get; } = new ObservableCollection<StatusItem>()
    {
        new() { Label = "Có sẵn", Value = "Available" },
        new() { Label = "Không có sẵn", Value = "Unavailable" }
    };

    /// <summary>
    /// Gets the collection of status item filters.
    /// </summary>
    public ObservableCollection<StatusItem> StatusItemsFilter { get; } = new ObservableCollection<StatusItem>()
    {
        new() { Label = "Tất cả", Value = null },
        new() { Label = "Có sẵn", Value = "Available" },
        new() { Label = "Không có sẵn", Value = "Unavailable" }
    };

    /// <summary>
    /// Gets or sets the collection of image URLs.
    /// </summary>
    public ObservableCollection<string> ImageUrls { get; set; } = new ObservableCollection<string>();

    public ObservableCollection<ProductOptionDto> ProductOptions { get; set; } = new ObservableCollection<ProductOptionDto>();

    /// <summary>
    /// Gets or sets the selected product.
    /// </summary>
    [ObservableProperty]
    private ProductDto? selectedProduct;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    [ObservableProperty]
    private ProductFilterDto productFilterDto = new();

    [ObservableProperty]
    private int perPage = 5;

    /// <summary>
    /// Gets or sets the current page number.
    /// </summary>
    [ObservableProperty]
    private int page = 1;

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    [ObservableProperty]
    private int totalPage = 1;

    /// <summary>
    /// Gets or sets the total number of records.
    /// </summary>
    [ObservableProperty]
    private long totalRecord = 0;
    
    [ObservableProperty]
    private SortDto sortDto = new();

    /// <summary>
    /// Gets or sets the action type.
    /// </summary>
    [ObservableProperty]
    private string? actionType;

    [ObservableProperty]
    private int tableHeight;

    [ObservableProperty]
    private string? productNameRequireMessage;

    [ObservableProperty]
    private string? productDescRequireMessage;

    [ObservableProperty]
    private string? productPriceRequireMessage;

    [ObservableProperty]
    private string? productCategoryRequireMessage;


    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductManagementViewModel"/> class.
    /// </summary>
    /// <param name="productService">The product service.</param>
    /// <param name="categoryService">The category service.</param>
    public ProductManagementViewModel(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    /// <summary>
    /// Searches for products based on the current filters and sorts.
    /// </summary>
    public async void UpdateProductList()
    {
        Products.Clear();
        Pagination<ProductDto> pagination = await _productService.GetWithPagination(ProductFilterDto, SortDto, Page, PerPage);
        Page = pagination.Page;
        PerPage = pagination.PerPage;
        TotalPage = pagination.TotalPage;
        TotalRecord = pagination.TotalRecord;
        foreach (var item in pagination.Data)
        {
            // If the product has no image associated with it
            // then use the default image
            if (item.ImageUris.Count == 0)
            {
                item.ImageUris.Add("ms-appx:///Assets/default_product_image.png");
            }

            Products.Add(item);
        }
    }

    /// <summary>
    /// Inserts the selected product.
    /// </summary>
    public async Task<ProductDto?> Insert()
    {
        if (SelectedProduct == null)
        {
            return null;
        }

        var isValidate = true;
        if (string.IsNullOrEmpty(SelectedProduct.Name))
        {
            ProductNameRequireMessage = "Tên sản phẩm không được để trống";
            isValidate = false;
        }
        if (string.IsNullOrEmpty(SelectedProduct.Description))
        {
            ProductDescRequireMessage = "Mô tả sản phẩm không được để trống";
            isValidate = false;
        }
        if (SelectedProduct.CategoryId == null)
        {
            ProductCategoryRequireMessage = "Danh mục sản phẩm không được để trống";
            isValidate = false;
        }
        SelectedProduct.ProductOptions.Clear();
        foreach (var item in ProductOptions)
        {
            if (item.Option == null || item.Price == null || double.IsNaN((double)item.Price) || item.Price < 0)
            {
                ProductPriceRequireMessage = "Giá sản phẩm không được để trống và phải lớn hơn hoặc bằng 0";
                isValidate = false;
                break;
            }
            SelectedProduct.ProductOptions.Add(new(item.Option, (decimal)item.Price));
        }
        if (SelectedProduct.ProductOptions.Count() == 0)
        {
            ProductPriceRequireMessage = "Phải có ít nhất một giá sản phẩm";
            isValidate = false;
        }
        if (!isValidate)
        {
            return null;
        }
        ProductDto? result = await _productService.Insert(SelectedProduct);
        UpdateProductList();
        return result;
    }

    /// <summary>
    /// Updates the selected product by its identifier.
    /// </summary>
    public async Task<ProductDto?> UpdateById()
    {
        if (SelectedProduct == null)
        {
            return null;
        }
        var isValidate = true;
        if (string.IsNullOrEmpty(SelectedProduct.Name))
        {
            ProductNameRequireMessage = "Tên sản phẩm không được để trống";
            isValidate = false;
        }
        if (string.IsNullOrEmpty(SelectedProduct.Description))
        {
            ProductDescRequireMessage = "Mô tả sản phẩm không được để trống";
            isValidate = false;
        }
        if (SelectedProduct.CategoryId == null)
        {
            ProductCategoryRequireMessage = "Danh mục sản phẩm không được để trống";
            isValidate = false;
        }
        SelectedProduct.ProductOptions.Clear();
        foreach (var item in ProductOptions)
        {
            if (item.Option == null || item.Price == null || double.IsNaN((double)item.Price) || item.Price < 0)
            {
                ProductPriceRequireMessage = "Giá sản phẩm không được để trống và phải lớn hơn hoặc bằng 0";
                isValidate = false;
                break;
            }
            SelectedProduct.ProductOptions.Add(new (item.Option,(decimal) item.Price));
        }
        if (SelectedProduct.ProductOptions.Count() == 0)
        {
            ProductPriceRequireMessage = "Phải có ít nhất một giá sản phẩm";
            isValidate = false;
        }
        if (!isValidate)
        {
            return null;
        }
        ProductDto? result = await _productService.UpdateById(SelectedProduct);
        UpdateProductList();
        return result;
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    public async void GetAllCategory()
    {
        Categories.Clear();
        CategoriesFilter.Clear();
        CategoriesFilter.Add(new CategoryDto { Id = null, Name = "Tất cả" });

        var data = await _categoryService.GetAll();

        foreach (var item in data)
        {
            Categories.Add(item);
            CategoriesFilter.Add(item);
        }
    }

    /// <summary>
    /// Deletes the selected products by their identifiers.
    /// </summary>
    public async void DeleteByIds()
    {
        List<long> ids = Products.Where(x => x.IsSelected && x.Id.HasValue).
                                  Select(x => x.Id.HasValue ? x.Id.Value : -1).
                                  ToList();

        await _productService.DeleteByIds(ids);
        UpdateProductList();
    }
}
