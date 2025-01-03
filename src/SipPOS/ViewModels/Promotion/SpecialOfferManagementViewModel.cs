using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using SipPOS.Services.Entity.Interfaces;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Models.General;
using Microsoft.UI.Xaml;

namespace SipPOS.ViewModels.Inventory;

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

    public SpecialOfferManagementViewModel(ISpecialOfferService specialOffersService, ICategoryService categoryService, IProductService productService)
    {
        _specialOffersService = specialOffersService;
        _categoryService = categoryService;
        _productService = productService;
    }

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
            SpecialOffers.Add(item);
        }
    }

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

    public async void DeleteByIds()
    {
        List<long> ids = SpecialOffers.Where(x => x.IsSelected && x.Id.HasValue).
                                  Select(x => x.Id.HasValue ? x.Id.Value : -1).
                                  ToList();

        await _specialOffersService.DeleteByIds(ids);
        UpdateSpecialOfferList();
    }

}
