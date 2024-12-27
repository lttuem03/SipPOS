using System.Collections.ObjectModel;
using System.ComponentModel;

using SipPOS.Resources.Helper;

using SipPOS.DataTransfer.Entity;
using SipPOS.Models.Entity;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.Configuration.Interfaces;
using SipPOS.Context.Configuration.Interfaces;
using Microsoft.UI.Xaml.Controls;

namespace SipPOS.ViewModels.Cashier;

public class CashierMenuViewModel : INotifyPropertyChanged
{
    // The way the current product list is loaded and displayed is
    // loading all the products of the store, and filter them using
    // direct LINQ queries. It works fine and smoothly now.

    // But this gets troublesome if the store has a large number of products,
    // then we need to implement Pagination.

    // Sample data
    //public TrulyObservableCollection<OrderItemDto> OrderItems { get; set; } = new()
    //{
    //    new() 
    //    { 
    //        Ordinal = 0,
    //        ItemName = "Phin Sữa",
    //        OptionName = "L",
    //        OptionPrice = 45_000m,
    //        Discount = 12_000,
    //        Note = ""
    //    },
    //    new()
    //    {
    //        Ordinal = 1,
    //        ItemName = "Trà Thanh Đào",
    //        OptionName = "M",
    //        OptionPrice = 35_000m,
    //        Discount = 0,
    //        Note = ""
    //    }
    //};
    //public OrderDto CurrentOrder { get; set; } = new()
    //{
    //    CreatedAt = DateTime.Now,
    //    ItemCount = 2,
    //    SubTotal = 80_000m,
    //    TotalDiscount = 12_000m,
    //    OrderBasedVAT = 0,
    //    Total = 68_000m
    //};

    public Models.General.Configuration CurrentConfiguration { get; private set; }

    public List<Category> Categories { get; set; }

    private List<Product> _productsOnDisplay;
    private List<Product> _allProducts;

    public TrulyObservableCollection<InvoiceItemDto> InvoiceItems { get; private set; } = new();    


    private readonly InvoiceDto _newInvoiceDto = new();

    public InvoiceItemDto? EditNoteTarget { get; set; } = null;
    
    // Data bound
    private string _vatRateString = string.Empty;
    private string _vatMessageString = string.Empty;
    
    public event PropertyChangedEventHandler? PropertyChanged;

    public CashierMenuViewModel()
    {
        var currentConfiguration = App.GetService<IConfigurationContext>().GetConfiguration();

        if (currentConfiguration == null)
        {
            throw new InvalidOperationException("Configuration is not loaded.");
        }

        CurrentConfiguration = currentConfiguration;

        Categories = new() { new() { Name = "Đang tải danh mục" } };

        _allProducts = new() { new() { Name = "Đang tải sản phẩm" } };
        _productsOnDisplay = _allProducts;

        // ignore warning here
        _ = Initialize();
    }

    private async Task Initialize()
    {
        var storeId = App.GetService<IStoreAuthenticationService>().GetCurrentStoreId();

        // Load tax configuration
        VatRateString = CurrentConfiguration.VatRate.ToString("P0");
        VatMessageString = CurrentConfiguration.VatMethod switch
        {
            "VAT_INCLUDED" => "(Giá bán sản phẩm đã bao gồm thuế VAT)",
            "ORDER_BASED" => "(Thuế VAT sẽ tính dựa trên tổng giá trị đơn hàng)",
            _ => "Phương thức VAT chưa xác định"
        };

        // Load categories
        var categoryDao = App.GetService<ICategoryDao>();
        Categories = await categoryDao.GetAllAsync(storeId);
        Categories.Insert(0, new() { Id = -1, Name = "Tất cả danh mục" });
        OnPropertyChanged(nameof(Categories));

        // Load all products
        var productDao = App.GetService<IProductDao>();
        _allProducts = await productDao.GetAllAsync(storeId);

        foreach (var product in _allProducts)
        {
            // If the product has no image associated with it
            // then use the default image
            if (product.ImageUris.Count == 0)
            {
                product.ImageUris.Add("ms-appx:///Assets/default_product_image.png");
            }

            // Select the first option of every product
            product.SelectedOption = product.ProductOptions[0];
        }

        // Initial "All categories" is chosen
        ProductsOnDisplay = _allProducts;

        // Initiate new invoice
        _newInvoiceDto.Id = await App.GetService<IInvoiceDao>().GetNextInvoiceIdAsync(storeId);
    }

    public void HandleItemSearchTextBoxTextChanged(string keywords)
    {
        ProductsOnDisplay = _allProducts.Where(p => p.Name.Contains(keywords)).ToList();
    }

    public void HandleCategoryBrowsingGridViewSelectionChanged(Category selectedCategory)
    {
        // "All category" selected
        if (selectedCategory.Id == -1)
        {
            ProductsOnDisplay = _allProducts;
            return;
        }

        ProductsOnDisplay = _allProducts.Where(p => p.CategoryId == selectedCategory.Id).ToList();
    }

    public void HandleAddItemToOrderButtonClick(Product productItem)
    {
        if (InvoiceItems.Count == 0)
            NewInvoiceCreatedAt = DateTime.Now;

        var invoiceItemDto = new InvoiceItemDto
        {
            Ordinal = InvoiceItems.Count + 1,
            ItemName = productItem.Name,
            OptionName = productItem.SelectedOption.name,
            OptionPrice = productItem.SelectedOption.price,
            Discount = 0,
            Note = ""
        };

        // Kinda weird adding to 2 lists, but it is nessessary
        InvoiceItems.Add(invoiceItemDto);           // this is to update the UI
        _newInvoiceDto.InvoiceItems.Add(invoiceItemDto); // this is to calculate order payment details

        // Update payment details
        NewInvoiceItemCount = InvoiceItems.Count;
        NewInvoiceSubTotal = InvoiceItems.Sum(orderItem => orderItem.OptionPrice);
        NewInvoiceTotalDiscount = InvoiceItems.Sum(orderItem => orderItem.Discount);

        NewInvoiceOrderBasedVAT = CurrentConfiguration.VatMethod switch
        {
            "ORDER_BASED" => NewInvoiceSubTotal * 0.05m,
            _ => 0m
        };

        NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceOrderBasedVAT;
    }

    public void HandleRemoveItemFromOrderButtonClick(InvoiceItemDto orderItemDto)
    {
        InvoiceItems.Remove(orderItemDto);
        _newInvoiceDto.InvoiceItems.Remove(orderItemDto);

        // Update the UI
        foreach (var orderItem in InvoiceItems)
        {
            orderItem.Ordinal = InvoiceItems.IndexOf(orderItem) + 1;
        }

        // Update payment details
        NewInvoiceItemCount = InvoiceItems.Count;
        NewInvoiceSubTotal = InvoiceItems.Sum(orderItem => orderItem.OptionPrice);
        NewInvoiceTotalDiscount = InvoiceItems.Sum(orderItem => orderItem.Discount);

        NewInvoiceOrderBasedVAT = CurrentConfiguration.VatMethod switch
        {
            "ORDER_BASED" => NewInvoiceSubTotal * 0.05m,
            _ => 0m
        };

        NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceOrderBasedVAT;

        if (InvoiceItems.Count == 0)
            NewInvoiceCreatedAt = DateTime.MinValue;
    }

    public async void HandleCancelOrderButtonClick(ContentDialog cancelOrderConfimationContentDialog)
    {
        if (InvoiceItems.Count == 0)
            return;

        var result = await cancelOrderConfimationContentDialog.ShowAsync();
    
        if (result == ContentDialogResult.Primary)
        {
            InvoiceItems.Clear();
            _newInvoiceDto.InvoiceItems.Clear();

            NewInvoiceCreatedAt = DateTime.MinValue;
            NewInvoiceItemCount = 0;
            NewInvoiceSubTotal = 0;
            NewInvoiceTotalDiscount = 0;
            NewInvoiceOrderBasedVAT = 0;
            NewInvoiceTotal = 0;
        }
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public string VatRateString
    {
        get => _vatRateString;
        set
        {
            _vatRateString = value;
            OnPropertyChanged(nameof(VatRateString));
        }
    }

    public string VatMessageString
    {
        get => _vatMessageString;
        set
        {
            _vatMessageString = value;
            OnPropertyChanged(nameof(VatMessageString));
        }
    }

    public List<Product> ProductsOnDisplay
    {
        get => _productsOnDisplay;
        set
        {
            _productsOnDisplay = value;
            OnPropertyChanged(nameof(ProductsOnDisplay));
        }
    }

    public long NewInvoiceId
    {
        get => _newInvoiceDto.Id;
        set
        {
            _newInvoiceDto.Id = value;
            OnPropertyChanged(nameof(NewInvoiceId));
        }
    }

    public DateTime NewInvoiceCreatedAt
    {
        get => _newInvoiceDto.CreatedAt;
        set
        {
            _newInvoiceDto.CreatedAt = value;
            OnPropertyChanged(nameof(NewInvoiceCreatedAt));
        }
    }

    public long NewInvoiceItemCount
    {
        get => _newInvoiceDto.ItemCount;
        set
        {
            _newInvoiceDto.ItemCount = value;
            OnPropertyChanged(nameof(NewInvoiceItemCount));
        }
    }

    public decimal NewInvoiceSubTotal
    {
        get => _newInvoiceDto.SubTotal;
        set
        {
            _newInvoiceDto.SubTotal = value;
            OnPropertyChanged(nameof(NewInvoiceSubTotal));
        }
    }

    public decimal NewInvoiceTotalDiscount
    {
        get => _newInvoiceDto.TotalDiscount;
        set
        {
            _newInvoiceDto.TotalDiscount = value;
            OnPropertyChanged(nameof(NewInvoiceTotalDiscount));
        }
    }

    public decimal NewInvoiceOrderBasedVAT
    {
        get => _newInvoiceDto.InvoiceBasedVAT;
        set
        {
            _newInvoiceDto.InvoiceBasedVAT = value;
            OnPropertyChanged(nameof(NewInvoiceOrderBasedVAT));
        }
    }

    public decimal NewInvoiceTotal
    {
        get => _newInvoiceDto.Total;
        set
        {
            _newInvoiceDto.Total = value;
            OnPropertyChanged(nameof(NewInvoiceTotal));
        }
    }
}