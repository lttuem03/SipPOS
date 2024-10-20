using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using SipPOS.Models;
using SipPOS.Services;
using SipPOS.DataTransfer;

namespace SipPOS.ViewModels;

public partial class ProductManagementViewModel : ObservableRecipient
{
    private readonly IProductService _productService;

    public ObservableCollection<ProductDto> Products { get; } = new ObservableCollection<ProductDto>();

    public ObservableCollection<ProductDto> SelectedProducts { get; } = new ObservableCollection<ProductDto>();

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

    public void Insert(ProductDto productDto)
    {
        _productService.Insert(productDto);
    }

}
