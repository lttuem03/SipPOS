using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using SipPOS.Models;
using SipPOS.Services;
using SipPOS.DataTransfer;

namespace SipPOS.ViewModels;

public partial class CategoryManagementViewModel : ObservableRecipient
{
    private readonly ICategoryService _categoryService;

    public ObservableCollection<CategoryDto> Categories { get; } = new ObservableCollection<CategoryDto>();

    public ObservableCollection<CategoryDto> SelectedCategories { get; } = new ObservableCollection<CategoryDto>();

    public CategoryManagementViewModel(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public void GetAll()
    {
        Categories.Clear();

        var data = _categoryService.GetAll();

        foreach (var item in data)
        {
            Categories.Add(item);
        }
    }

    public void Insert(CategoryDto categoryDto)
    {
        _categoryService.Insert(categoryDto);
    }

}
