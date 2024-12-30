using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using SipPOS.Services.Entity.Interfaces;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Models.General;

namespace SipPOS.ViewModels.Inventory;

public partial class SpecialOfferManagementViewModel : ObservableRecipient
{

    public ObservableCollection<SpecialOfferDto> SpecialOffers { get; } = new ObservableCollection<SpecialOfferDto>();

    public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();

    public ObservableCollection<CategoryDto> CategoriesFilter { get; } = new ObservableCollection<CategoryDto>();

    public ObservableCollection<StatusItem> StatusItems { get; } = new ObservableCollection<StatusItem>()
    {
        new() { Label = "Chưa áp dụng", Value = "Inactive" },
        new() { Label = "Đang hoạt động", Value = "Active" },
        new() { Label = "Đã hết hạn", Value = "Expired" },
        new() { Label = "Đã hủy", Value = "Cancelled" }
    };

    public ObservableCollection<StatusItem> StatusItemsFilter { get; } = new ObservableCollection<StatusItem>()
    {
        new() { Label = "Tất cả", Value = null },
        new() { Label = "Chưa áp dụng", Value = "Inactive" },
        new() { Label = "Đang hoạt động", Value = "Active" },
        new() { Label = "Đã hết hạn", Value = "Expired" },
        new() { Label = "Đã hủy", Value = "Cancelled" }
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
    private string? specialOfferNameRequireMessage;

    [ObservableProperty]
    private string? specialOfferDescRequireMessage;

    [ObservableProperty]
    private string? specialOfferPriceRequireMessage;

    [ObservableProperty]
    private string? specialOfferCategoryRequireMessage;


    private readonly ISpecialOfferService _specialOffersService;
    private readonly ICategoryService _categoryService;

    public SpecialOfferManagementViewModel(ISpecialOfferService specialOffersService, ICategoryService categoryService)
    {
        _specialOffersService = specialOffersService;
        _categoryService = categoryService;
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
        if (string.IsNullOrEmpty(SelectedSpecialOffer.Name))
        {
            SpecialOfferNameRequireMessage = "Tên sản phẩm không được để trống";
            isValidate = false;
        }
        if (string.IsNullOrEmpty(SelectedSpecialOffer.Description))
        {
            SpecialOfferDescRequireMessage = "Mô tả sản phẩm không được để trống";
            isValidate = false;
        }
        if (SelectedSpecialOffer.CategoryId == null)
        {
            SpecialOfferCategoryRequireMessage = "Danh mục sản phẩm không được để trống";
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
        if (string.IsNullOrEmpty(SelectedSpecialOffer.Name))
        {
            SpecialOfferNameRequireMessage = "Tên sản phẩm không được để trống";
            isValidate = false;
        }
        if (string.IsNullOrEmpty(SelectedSpecialOffer.Description))
        {
            SpecialOfferDescRequireMessage = "Mô tả sản phẩm không được để trống";
            isValidate = false;
        }
        if (SelectedSpecialOffer.CategoryId == null)
        {
            SpecialOfferCategoryRequireMessage = "Danh mục sản phẩm không được để trống";
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

    public async void DeleteByIds()
    {
        List<long> ids = SpecialOffers.Where(x => x.IsSelected && x.Id.HasValue).
                                  Select(x => x.Id.HasValue ? x.Id.Value : -1).
                                  ToList();

        await _specialOffersService.DeleteByIds(ids);
        UpdateSpecialOfferList();
    }
}
