using System.Collections.ObjectModel;

using Microsoft.UI.Xaml;

using CommunityToolkit.Mvvm.ComponentModel;

using SipPOS.Models.General;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Services.Entity.Interfaces;

namespace SipPOS.ViewModels.Inventory;

/// <summary>
/// ViewModel for managing special offers, including CRUD operations and filtering.
/// </summary>
public partial class SpecialOfferManagementViewModel : ObservableRecipient
{
    public ObservableCollection<SpecialOfferDto> SpecialOffers { get; } = new ObservableCollection<SpecialOfferDto>();
    public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();
    public ObservableCollection<ProductDto> Products { get; } = new ObservableCollection<ProductDto>();
    public ObservableCollection<ProductDto> ProductsFilter { get; } = new ObservableCollection<ProductDto>();
    public ObservableCollection<CategoryDto> CategoriesFilter { get; } = new ObservableCollection<CategoryDto>();
    public ObservableCollection<StatusItem> StatusItems { get; } = new ObservableCollection<StatusItem>()
    {
        new() { Label = "Chưa áp dụng", Value = "Inactive" },
        new() { Label = "Áp dụng", Value = "Active" },
        new() { Label = "Đã hết hạn", Value = "Expired" },
        new() { Label = "Đã hủy", Value = "Cancelled" }
    };

    public ObservableCollection<StatusItem> StatusItemsFilter { get; } = new ObservableCollection<StatusItem>()
    {
        new() { Label = "Tất cả", Value = null },
        new() { Label = "Chưa áp dụng", Value = "Inactive" },
        new() { Label = "Áp dụng", Value = "Active" },
        new() { Label = "Đã hết hạn", Value = "Expired" },
        new() { Label = "Đã hủy", Value = "Cancelled" }
    };

    public ObservableCollection<SpecialOfferType> SpecialOfferTypes { get; } = new ObservableCollection<SpecialOfferType>()
    {
        new() { Label = "Khuyến mãi trên sản phẩm", Value = "ProductPromotion" },
        new() { Label = "Khuyến mãi trên danh mục sản phẩm", Value = "CategoryPromotion" },
        new() { Label = "Khuyến mãi trên hóa đơn", Value = "InvoicePromotion" },
    };

    public ObservableCollection<SpecialOfferType> PriceTypes { get; } = new ObservableCollection<SpecialOfferType>()
    {
        new() { Label = "Giảm theo giá", Value = "Original" },
        new() { Label = "Giảm theo phần trăm", Value = "Percentage" },
    };

    [ObservableProperty]
    private SpecialOfferDto? selectedSpecialOffer;

    [ObservableProperty]
    private SpecialOfferFilterDto specialOffersFilterDto = new();

    [ObservableProperty]
    private int perPage = 5;

    [ObservableProperty]
    private int page = 1;

    [ObservableProperty]
    private int totalPage = 1;

    [ObservableProperty]
    private long totalRecord = 0;
    
    [ObservableProperty]
    private SortDto sortDto = new();

    [ObservableProperty]
    private string? actionType;

    [ObservableProperty]
    private int tableHeight;

    [ObservableProperty]
    private string? specialOfferCodeRequireMessage;

    [ObservableProperty]
    private string? specialOfferNameRequireMessage;

    [ObservableProperty]
    private string? specialOfferDescRequireMessage;

    [ObservableProperty]
    private string? specialOfferStartDateRequireMessage;

    [ObservableProperty]
    private string? specialOfferPriceRequireMessage;

    [ObservableProperty]
    private string? specialOfferTypeRequireMessage;

    [ObservableProperty]
    private string? specialOfferMaxItemsRequireMessage;

    [ObservableProperty]
    private Visibility discountPriceVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility discountPecentageVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility productPromotionVisibility = Visibility.Collapsed;

    [ObservableProperty]
    private Visibility categoryPromotionVisibility = Visibility.Collapsed;


    private readonly ISpecialOfferService _specialOffersService;
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SpecialOfferManagementViewModel"/> class.
    /// </summary>
    /// <param name="specialOffersService">The service for managing special offers.</param>
    /// <param name="categoryService">The service for managing categories.</param>
    /// <param name="productService">The service for managing products.</param>
    public SpecialOfferManagementViewModel(ISpecialOfferService specialOffersService, ICategoryService categoryService, IProductService productService)
    {
        _specialOffersService = specialOffersService;
        _categoryService = categoryService;
        _productService = productService;
    }

    /// <summary>
    /// Updates the list of special offers with pagination and sorting.
    /// </summary>
    public async void UpdateSpecialOfferList()
    {
        SpecialOffers.Clear();
        Pagination<SpecialOfferDto> pagination = await _specialOffersService.GetWithPagination(SpecialOffersFilterDto, SortDto, Page, PerPage);
        Page = pagination.Page;
        PerPage = pagination.PerPage;
        TotalPage = pagination.TotalPage;
        TotalRecord = pagination.TotalRecord;
        foreach (var item in pagination.Data)
        {
            if (item.PriceType == "Original")
            {
                if (item.DiscountPrice == null)
                    item.DiscountAmountString = "0₫";
                else
                    item.DiscountAmountString = item.DiscountPrice.Value.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
            }
            else if (item.PriceType == "Percentage")
            {
                if (item.DiscountPercentage == null)
                    item.DiscountAmountString = "0%";
                else
                    item.DiscountAmountString = $"{item.DiscountPercentage:0}%";
            }

            SpecialOffers.Add(item);
        }
    }

    /// <summary>
    /// Inserts a new special offer.
    /// </summary>
    /// <returns>The inserted special offer, or null if validation fails.</returns>
    public async Task<SpecialOfferDto?> Insert()
    {
        if (SelectedSpecialOffer == null)
        {
            return null;
        }

        var isValidate = true;
        if (string.IsNullOrEmpty(SelectedSpecialOffer.Code))
        {
            SpecialOfferCodeRequireMessage = "Mã khuyến mãi không được để trống";
            isValidate = false;
        }
        if (string.IsNullOrEmpty(SelectedSpecialOffer.Name))
        {
            SpecialOfferNameRequireMessage = "Tên khuyến mãi không được để trống";
            isValidate = false;
        }
        if (string.IsNullOrEmpty(SelectedSpecialOffer.Description))
        {
            SpecialOfferDescRequireMessage = "Mô tả khuyến mãi không được để trống";
            isValidate = false;
        }
        if (SelectedSpecialOffer.CategoryId == null && SelectedSpecialOffer.Type == "CategoryPromotion")
        {
            SpecialOfferTypeRequireMessage = "Danh mục khuyến mãi không được để trống";
            isValidate = false;
        }
        if (SelectedSpecialOffer.ProductId == null && SelectedSpecialOffer.Type == "ProductPromotion")
        {
            SpecialOfferTypeRequireMessage = "Sản phẩm khuyến mãi không được để trống";
            isValidate = false;
        }
        if (SelectedSpecialOffer.PriceType == "Original" && SelectedSpecialOffer.DiscountPrice <= 0)
        {
            SpecialOfferPriceRequireMessage = "Giá khuyến mãi không được nhỏ hơn hoặc bằng 0";
            isValidate = false;
        }
        if (SelectedSpecialOffer.PriceType == "Percentage" && SelectedSpecialOffer.DiscountPercentage <= 0)
        {
            SpecialOfferTypeRequireMessage = "Phần trăm khuyến mãi không được nhỏ hơn hoặc bằng 0";
            isValidate = false;
        }
        if (SelectedSpecialOffer.StartDate > SelectedSpecialOffer.EndDate)
        {
            SpecialOfferStartDateRequireMessage = "Ngày bắt đầu không được lớn hơn ngày kết thúc";
            isValidate = false;
        }
        if (SelectedSpecialOffer.MaxItems <= 0)
        {
            SpecialOfferMaxItemsRequireMessage = "Số lượng tối đa không được nhỏ hơn hoặc bằng 0";
            isValidate = false;
        }
        if (!isValidate)
        {
            return null;
        }
        SpecialOfferDto? result = await _specialOffersService.Insert(SelectedSpecialOffer);
        UpdateSpecialOfferList();
        return result;
    }

    /// <summary>
    /// Updates an existing special offer by its ID.
    /// </summary>
    /// <returns>The updated special offer, or null if validation fails.</returns>
    public async Task<SpecialOfferDto?> UpdateById()
    {
        if (SelectedSpecialOffer == null)
        {
            return null;
        }
        var isValidate = true;
        if (string.IsNullOrEmpty(SelectedSpecialOffer.Code))
        {
            SpecialOfferCodeRequireMessage = "Mã khuyến mãi không được để trống";
            isValidate = false;
        }
        if (string.IsNullOrEmpty(SelectedSpecialOffer.Name))
        {
            SpecialOfferNameRequireMessage = "Tên khuyến mãi không được để trống";
            isValidate = false;
        }
        if (string.IsNullOrEmpty(SelectedSpecialOffer.Description))
        {
            SpecialOfferDescRequireMessage = "Mô tả khuyến mãi không được để trống";
            isValidate = false;
        }
        if (SelectedSpecialOffer.CategoryId == null && SelectedSpecialOffer.Type == "CategoryPromotion")
        {
            SpecialOfferTypeRequireMessage = "Danh mục khuyến mãi không được để trống";
            isValidate = false;
        }
        if (SelectedSpecialOffer.ProductId == null && SelectedSpecialOffer.Type == "ProductPromotion")
        {
            SpecialOfferTypeRequireMessage = "Sản phẩm khuyến mãi không được để trống";
            isValidate = false;
        }
        if (SelectedSpecialOffer.PriceType == "Percentage" && SelectedSpecialOffer.DiscountPercentage <= 0)
        {
            SpecialOfferTypeRequireMessage = "Phần trăm khuyến mãi không được nhỏ hơn hoặc bằng 0";
            isValidate = false;
        }
        if (SelectedSpecialOffer.StartDate > SelectedSpecialOffer.EndDate)
        {
            SpecialOfferStartDateRequireMessage = "Ngày bắt đầu không được lớn hơn ngày kết thúc";
            isValidate = false;
        }
        if (SelectedSpecialOffer.MaxItems <= 0)
        {
            SpecialOfferMaxItemsRequireMessage = "Số lượng tối đa không được nhỏ hơn hoặc bằng 0";
            isValidate = false;
        }
        if (!isValidate)
        {
            return null;
        }
        SpecialOfferDto? result = await _specialOffersService.UpdateById(SelectedSpecialOffer);
        UpdateSpecialOfferList();
        return result;
    }

    /// <summary>
    /// Retrieves all categories and updates the Categories and CategoriesFilter collections.
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
    /// Retrieves all products and updates the Products and ProductsFilter collections.
    /// </summary>
    public async void GetAllProduct()
    {
        Products.Clear();
        ProductsFilter.Clear();
        ProductsFilter.Add(new ProductDto { Id = null, Name = "Tất cả" });

        var data = await _productService.GetAll();

        foreach (var item in data)
        {
            Products.Add(item);
            ProductsFilter.Add(item);
        }
    }

    /// <summary>
    /// Deletes special offers by their IDs and updates the special offer list.
    /// </summary>
    public async void DeleteByIds()
    {
        List<long> ids = SpecialOffers.Where(x => x.IsSelected && x.Id.HasValue).
                                  Select(x => x.Id.HasValue ? x.Id.Value : -1).
                                  ToList();

        await _specialOffersService.DeleteByIds(ids);
        UpdateSpecialOfferList();
    }

}
