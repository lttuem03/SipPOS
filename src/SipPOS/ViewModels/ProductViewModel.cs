using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using SipPOS.Models;
using SipPOS.Services;

namespace SipPOS.ViewModels;

public partial class ProductViewModel : ObservableRecipient
{
    private readonly IProductService _productService;

    public ObservableCollection<Product> Products { get; } = new ObservableCollection<Product>();

    public ObservableCollection<Product> SelectedProducts { get; } = new ObservableCollection<Product>();

    public ProductViewModel(IProductService productService)
    {
        _productService = productService;
    }

    public async void Get()
    {
        Products.Clear();

        var data = await _productService.Get();

        foreach (var item in data)
        {
            Products.Add(item);
        }
    }

    public async void Add(Product product)
    {
        await _productService.Add(product);
    }

}
