using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using SipPOS.Services.Entity.Interfaces;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Models.General;

namespace SipPOS.ViewModels.Management;

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
    private int perPage = 10;

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
    public void Search()
    {
        Products.Clear();
        Pagination<ProductDto> pagination = _productService.Search(ProductFilterDto, SortDto, Page, PerPage);
        Page = pagination.Page;
        PerPage = pagination.PerPage;
        TotalPage = pagination.TotalPage;
        TotalRecord = pagination.TotalRecord;
        foreach (var item in pagination.Data)
        {
            Products.Add(item);
        }
    }

    /// <summary>
    /// Inserts the selected product.
    /// </summary>
    public void Insert()
    {
        if (SelectedProduct == null)
        {
            return;
        }

        _productService.Insert(SelectedProduct);
        Search();
    }

    /// <summary>
    /// Updates the selected product by its identifier.
    /// </summary>
    public void UpdateById()
    {
        if (SelectedProduct == null)
        {
            return;
        }

        _productService.UpdateById(SelectedProduct);
        Search();
    }

    /// <summary>
    /// Gets all categories.
    /// </summary>
    public void GetAllCategory()
    {
        Categories.Clear();
        CategoriesFilter.Clear();
        CategoriesFilter.Add(new CategoryDto { Id = null, Name = "Tất cả" });

        var data = _categoryService.GetAll();
        foreach (var item in data)
        {
            Categories.Add(item);
            CategoriesFilter.Add(item);
        }
    }

    /// <summary>
    /// Deletes the selected products by their identifiers.
    /// </summary>
    public void DeleteByIds()
    {
        List<long> ids = Products.Where(x => x.IsSeteled && x.Id.HasValue).
                                  Select(x => x.Id.HasValue ? x.Id.Value : -1).
                                  ToList();

        _productService.DeleteByIds(ids);
        Search();
    }
}
