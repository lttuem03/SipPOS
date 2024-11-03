using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using SipPOS.Models;
using SipPOS.Services;
using SipPOS.DataTransfer;

namespace SipPOS.ViewModels;

public partial class CategoryManagementViewModel : ObservableRecipient
{
    public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();
    public ObservableCollection<StatusItem> StatusItems { get; } = new ObservableCollection<StatusItem>()
    {
        new() { Label = "Có sẵn", Value = "Available" },
        new() { Label = "Không có sẵn", Value = "Unavailable" }
    };
    public ObservableCollection<string> ImageUrls { get; set; } = new ObservableCollection<string>();

    [ObservableProperty]
    private CategoryDto? selectedCategory;

    [ObservableProperty]
    private CategoryFilterDto? categoryFilterDto = new CategoryFilterDto();

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

    private readonly ICategoryService _categoryService;

    public CategoryManagementViewModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public void Search()
    {
        Categories.Clear();
        Pagination<CategoryDto> pagination = _categoryService.Search(CategoryFilterDto, SortDto, Page, PerPage);
        Page = pagination.Page;
        PerPage = pagination.PerPage;
        TotalPage = pagination.TotalPage;
        TotalRecord = pagination.TotalRecord;
        foreach (var item in pagination.Data)
        {
            Categories.Add(item);
        }
    }

    public void Insert()
    {
        if (SelectedCategory == null)
        {
            return;
        }

        _categoryService.Insert(SelectedCategory);
        Search();
    }

    public void UpdateById()
    {
        if (SelectedCategory == null)
        {
            return;
        }

        _categoryService.UpdateById(SelectedCategory);
        Search();
    }

    public void DeleteByIds()
    {
        List<long> ids = Categories.Where(x => x.IsSeteled && x.Id.HasValue).
                                    Select(x => x.Id.HasValue ? x.Id.Value : -1).
                                    ToList();

        _categoryService.DeleteByIds(ids);
        Search();
    }
}