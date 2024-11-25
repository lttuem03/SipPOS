using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

using SipPOS.Services.Entity.Interfaces;
using SipPOS.DataTransfer.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Models.General;

namespace SipPOS.ViewModels.Management;

/// <summary>
/// ViewModel for managing categories, handling data for data-binding and the logic in the CategoryManagementView.
/// </summary>
public partial class CategoryManagementViewModel : ObservableRecipient
{
    /// <summary>
    /// Gets the collection of categories.
    /// </summary>
    public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();

    /// <summary>
    /// Gets the collection of status items.
    /// </summary>
    public ObservableCollection<StatusItem> StatusItems { get; } = new ObservableCollection<StatusItem>()
    {
        new() { Label = "Có sẵn", Value = "Available" },
        new() { Label = "Không có sẵn", Value = "Unavailable" }
    };

    /// <summary>
    /// Gets or sets the collection of image URLs.
    /// </summary>
    public ObservableCollection<string> ImageUrls { get; set; } = new ObservableCollection<string>();

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
    /// Gets or sets the selected category.
    /// </summary>
    [ObservableProperty]
    private CategoryDto? selectedCategory;

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    [ObservableProperty]
    private CategoryFilterDto categoryFilterDto = new();

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

    /// <summary>
    /// Gets or sets the action type.
    /// </summary>
    [ObservableProperty]
    private SortDto sortDto = new();

    [ObservableProperty]
    public string? actionType;

    private readonly ICategoryService _categoryService;

    /// <summary>
    /// Initializes a new instance of the <see cref="CategoryManagementViewModel"/> class.
    /// </summary>
    /// <param name="categoryService">The category service.</param>
    public CategoryManagementViewModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    /// <summary>
    /// Searches for categories based on the current filters and sorts.
    /// </summary>
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

    /// <summary>
    /// Inserts the selected category.
    /// </summary>
    public void Insert()
    {
        if (SelectedCategory == null)
        {
            return;
        }

        _categoryService.Insert(SelectedCategory);
        Search();
    }

    /// <summary>
    /// Updates the selected category by its identifier.
    /// </summary>
    public void UpdateById()
    {
        if (SelectedCategory == null)
        {
            return;
        }

        _categoryService.UpdateById(SelectedCategory);
        Search();
    }

    /// <summary>
    /// Deletes the selected categories by their identifiers.
    /// </summary>
    public void DeleteByIds()
    {
        List<long> ids = Categories.Where(x => x.IsSeteled && x.Id.HasValue).
                                    Select(x => x.Id.HasValue ? x.Id.Value : -1).
                                    ToList();

        _categoryService.DeleteByIds(ids);
        Search();
    }
}
