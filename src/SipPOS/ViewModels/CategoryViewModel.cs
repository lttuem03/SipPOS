using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using SipPOS.Models;
using SipPOS.Services;

namespace SipPOS.ViewModels;

public partial class CategoryViewModel : ObservableRecipient
{
    private readonly ICategoryService _categoryService;

    public ObservableCollection<Category> Categories { get; } = new ObservableCollection<Category>();

    public ObservableCollection<Category> SelectedCategories { get; } = new ObservableCollection<Category>();

    public CategoryViewModel(ICategoryService categoryService)
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

    public void Insert(Category category)
    {
        _categoryService.Insert(category);
    }

}
