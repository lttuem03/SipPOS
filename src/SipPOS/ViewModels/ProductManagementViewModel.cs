using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using SipPOS.Models;
using SipPOS.Services;
using SipPOS.DataTransfer;
using SipPOS.Views;
using System.ComponentModel;
using System.Collections.Immutable;
using WinRT;

namespace SipPOS.ViewModels;

public partial class ProductManagementViewModel : ObservableRecipient
{
    public ObservableCollection<ProductDto> Products { get; } = new ObservableCollection<ProductDto>();
    public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();
    public ObservableCollection<StatusItem> StatusItems { get; } = new ObservableCollection<StatusItem>()
    {
        new() { Label = "Có sẵn", Value = "Available" },
        new() { Label = "Không có sẵn", Value = "Unavailable" }
    };
    public ObservableCollection<string> ImageUrls { get; set; } = new ObservableCollection<string>();

    [ObservableProperty]
    private ProductDto? selectedProduct;

    [ObservableProperty]
    private ProductFilterDto? productFilterDto = new ProductFilterDto();

    [ObservableProperty]
    private int perPage = 5;

    [ObservableProperty]
    private int page = 1;

    [ObservableProperty]
    private int totalPage = 1;

    [ObservableProperty]
    private long totalRecord = 0;
    
    [ObservableProperty]
    private SortDto sortDto = new SortDto();

    [ObservableProperty]
    public string? actionType;

    private readonly IProductService _productService;

    private readonly ICategoryService _categoryService;

    public ProductManagementViewModel(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

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

    public void Insert()
    {
        if (SelectedProduct == null)
        {
            return;
        }

        _productService.Insert(SelectedProduct);
        Search();
    }

    public void UpdateById()
    {
        if (SelectedProduct == null)
        {
            return;
        }

        _productService.UpdateById(SelectedProduct);
        Search();
    }

    public void GetAllCategory()
    {
        Categories.Clear();

        var data = _categoryService.GetAll();
        foreach (var item in data)
        {
            Categories.Add(item);
        }
    }

    public void DeleteByIds()
    {
        List<long> ids = Products.Where(x => x.IsSeteled && x.Id.HasValue).
                                  Select(x => x.Id.HasValue ? x.Id.Value : -1).
                                  ToList();

        _productService.DeleteByIds(ids);
        Search();
    }
}
