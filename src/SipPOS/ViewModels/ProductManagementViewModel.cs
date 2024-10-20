using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using SipPOS.Models;
using SipPOS.Services;

namespace SipPOS.ViewModels;

public partial class ProductManagementViewModel : ObservableRecipient
{
    private readonly IProductService _productService;

    public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

    public ObservableCollection<Product> SelectedProducts { get; } = new ObservableCollection<Product>();

    public ProductManagementViewModel(IProductService productService)
    {
        _productService = productService;
    }

    public void GetAll()
    {
        Products.Clear();

        var data = _productService.GetAll();

        foreach (var item in data)
        {
            Products.Add(item);
        }
    }

    public void Insert(Product product)
    {
        _productService.Insert(product);
    }

}
