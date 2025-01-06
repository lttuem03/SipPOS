using System.Collections.ObjectModel;
using System.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using SipPOS.Resources.Helper;
using SipPOS.DataTransfer.Entity;
using SipPOS.Models.Entity;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.Configuration.Interfaces;
using SipPOS.Context.Configuration.Interfaces;
using SipPOS.Services.Authentication.Interfaces;
using System;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.Authentication.Implementations;
using Microsoft.VisualBasic;
using Microsoft.UI.Xaml;
using System.Linq;

using Microsoft.UI.Xaml.Media.Imaging;
using Net.payOS;
using Net.payOS.Types;
using System.Drawing;
using QRCoder;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.Entity.Implementations;


namespace SipPOS.ViewModels.Cashier;

public class CashierMenuViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    // The way the current product list is loaded and displayed is
    // loading all the products of the store, and filter them using
    // direct LINQ queries. It works fine and smoothly now.

    // But this gets troublesome if the store has a large number of products,
    // then we need to implement Pagination.

    // Contextual properties
    public Models.Entity.Store CurrentStore { get; private set; }
    public Models.Entity.Staff CurrentStaff { get; private set; }
    public Models.General.Configuration CurrentConfiguration { get; private set; }
    public DateTime CurrentTime { get; set; }
    public InvoiceItemDto? EditNoteTarget { get; set; } = null;
    public TimeSpan CancelQrPayOperationCountDownTimeSpan { get; set; } = TimeSpan.Zero;

    // Data-bound properties
    private List<Category> _categories = new() { new() { Name = "Đang tải danh mục" } };
    private List<Product> _allProducts = new() { new() { Name = "Đang tải sản phẩm" } };
    private List<Product> _productsOnDisplay;

    private TrulyObservableCollection<InvoiceItemDto> _invoiceItems = new();
    private readonly InvoiceDto _newInvoiceDto = new();

    private string _vatRateString = string.Empty;
    private string _vatMessageString = string.Empty;

    // QR-Pay properties
    private readonly long QRPAY_EXPIRE_DURATION_IN_SECONDS = 300; // 5 min

    private BitmapImage _qrCode = new(new Uri("ms-appx:///Assets/blank_qr.png"));
    private long _qrPaySecondsRemaining = 0;
    private long _qrPayOrderCode = 0;
    private string _qrPayAccountNumber = string.Empty;
    private string _qrCodeData = string.Empty;

    private string _qrPayOrderCodeString = string.Empty;

    private ITextToSpeechService textToSpeechService;

    private readonly PayOS payOsClient;

    public CashierMenuViewModel()
    {
        var currentConfiguration = App.GetService<IConfigurationContext>().GetConfiguration();

        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
            throw new InvalidOperationException("Store authentication service is not registered.");

        if (App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
            throw new InvalidOperationException("Staff authentication service is not registered.");

        if (currentConfiguration == null)
            throw new InvalidOperationException("Configuration is not loaded.");

        if (storeAuthenticationService.Context.CurrentStore == null)
            throw new InvalidOperationException("Store is not logged in.");

        if (staffAuthenticationService.Context.CurrentStaff == null)
            throw new InvalidOperationException("Staff is not logged in.");

        CurrentStore = storeAuthenticationService.Context.CurrentStore;
        CurrentStaff = staffAuthenticationService.Context.CurrentStaff;
        CurrentConfiguration = currentConfiguration;
        CurrentTime = DateTime.Now;

        _productsOnDisplay = _allProducts;

        // initialize PayOS
        var clientId = DotNetEnv.Env.GetString("PAYOS_CLIENT_ID");
        var apiKey = DotNetEnv.Env.GetString("PAYOS_API_KEY");
        var checksumKey = DotNetEnv.Env.GetString("PAYOS_CHECKSUM_KEY");

        payOsClient = new PayOS(clientId, apiKey, checksumKey);

        textToSpeechService = App.GetService<ITextToSpeechService>();

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

    public InvoiceItemDto HandleAddItemToOrderButtonClick(Product productItem)
    {
        if (InvoiceItems.Count == 0)
            NewInvoiceCreatedAt = DateTime.Now;

        var invoiceItemDto = new InvoiceItemDto
        {
            Ordinal = InvoiceItems.Count + 1,
            ProductId = productItem.Id,
            ItemName = productItem.Name,
            OptionName = productItem.SelectedOption.name,
            OptionPrice = productItem.SelectedOption.price,
            Discount = 0,
            Note = ""
        };

        InvoiceItems.Add(invoiceItemDto);

        // Update payment details
        NewInvoiceItemCount = InvoiceItems.Count;
        NewInvoiceSubTotal = InvoiceItems.Sum(orderItem => orderItem.OptionPrice);
        NewInvoiceTotalDiscount = InvoiceItems.Sum(orderItem => orderItem.Discount);

        NewInvoiceInvoiceBasedVAT = CurrentConfiguration.VatMethod switch
        {
            "ORDER_BASED" => NewInvoiceSubTotal * 0.05m,
            _ => 0m
        };

        NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceInvoiceBasedVAT;

        return invoiceItemDto;
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

        NewInvoiceInvoiceBasedVAT = CurrentConfiguration.VatMethod switch
        {
            "ORDER_BASED" => NewInvoiceSubTotal * 0.05m,
            _ => 0m
        };

        NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceInvoiceBasedVAT;

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
            NewInvoiceInvoiceBasedVAT = 0;
            NewInvoiceTotal = 0;
        }
    }

    public void HandlePaymentMethodRadioButtonsSelectionChanged(int selectedIndex)
    {
        ResetPaymentMonetaryDetails();

        switch (selectedIndex)
        {
            case 0:
                NewInvoicePaymentMethod = "CASH";
                break;
            case 1:
                NewInvoicePaymentMethod = "QR_PAY";
                break;
        }
    }

    public void HandleNumpadAddAmountButtonClick(decimal amount)
    {
        NewInvoiceCustomerPaid += amount;
        NewInvoiceChange = Math.Max(0, NewInvoiceCustomerPaid - NewInvoiceTotal);
    }

    public void HandleNumpadNumberButtonClick(string numberText)
    {
        if (numberText == "000")
        {
            NewInvoiceCustomerPaid *= 1000;
            NewInvoiceChange = Math.Max(0, NewInvoiceCustomerPaid - NewInvoiceTotal);
            return;
        }

        var number = decimal.Parse(numberText);

        NewInvoiceCustomerPaid *= 10;
        NewInvoiceCustomerPaid += number;

        NewInvoiceChange = Math.Max(0, NewInvoiceCustomerPaid - NewInvoiceTotal);
    }

    public void HandleNumpadClearAmountButtonClick()
    {
        ResetPaymentMonetaryDetails();
    }

    public void HandleNumpadBackspaceButtonClick()
    {
        NewInvoiceCustomerPaid = Math.Floor(NewInvoiceCustomerPaid / 10);
        NewInvoiceChange = Math.Max(0, NewInvoiceCustomerPaid - NewInvoiceTotal);
    }

    public async Task ProceedWithPayment()
    {
        // All validation passed at this point
        var updateItemsSoldProductIds = new List<long>();
        var productDao = App.GetService<IProductDao>();

        // Update current staff name and id to the new invoice
        _newInvoiceDto.StaffId = CurrentStaff.Id;
        _newInvoiceDto.StaffName = CurrentStaff.Name;

        // Adding invoice items to the new invoice
        _newInvoiceDto.InvoiceItems.Clear();

        foreach (var invoiceItem in InvoiceItems)
        {
            invoiceItem.InvoiceId = NewInvoiceId;
            
            _newInvoiceDto.InvoiceItems.Add(invoiceItem);

            // Update ItemsSold for the products
            var product = await productDao.GetByIdAsync(CurrentStore.Id, invoiceItem.ProductId);

            if (product == null)
            {
                // DO NOT THROW EXCEPTION HERE, BECAUSE
                // WHEN THERE'S MONEY INVOLVE, BE EXTRA CAREFUL,
                // If the product is not found, then just skip it
                continue;
            }

            product.ItemsSold++;
            product.UpdatedAt = DateTime.Now;
            product.UpdatedBy = CurrentStaff.CompositeUsername;

            var updatedProduct = await productDao.UpdateByIdAsync(CurrentStore.Id, product);

            if (updatedProduct == null)
            {
                // DO NOT THROW EXCEPTION, SAME REASON AS ABOVE
                continue;
            }

            if (!updateItemsSoldProductIds.Contains(product.Id))
                updateItemsSoldProductIds.Add(product.Id);
        }

        // Insert to database
        var invoiceDao = App.GetService<IInvoiceDao>();

        var result = await invoiceDao.InsertAsync(CurrentStore.Id, _newInvoiceDto);

        if (result == null)
        {
            // IF THE PAYMENT WENT THROUGH, BUT THE INVOICE INSERTION FAILED
            // MIGHT AS WELL BREAK THE APP, TO STOP ANY FURTHER PAYMENT

            // ASK CUSTOMER TO CALL IT DEPARTMENT (AKA US) FOR HELP

            throw new InvalidOperationException("Failed to insert new invoice");
        }
            

        // Update the _allProducts where the "ItemsSold" for the product has changed
        foreach (var productId in updateItemsSoldProductIds)
        {
            var productToUpdate = _allProducts.First(product => product.Id == productId);

            var productInDatabase = await productDao.GetByIdAsync(CurrentStore.Id, productId);

            if (productInDatabase == null)
            {
                // DO NOT THROW EXCEPTION, SAME REASON AS ABOVE
                continue;
            }

            productToUpdate.ItemsSold = productInDatabase.ItemsSold;
        }

        ProductsOnDisplay = _allProducts;

        // A litte trick to update the UI
        HandleCategoryBrowsingGridViewSelectionChanged(Categories[1]);
        HandleCategoryBrowsingGridViewSelectionChanged(Categories[0]);

        NotifyPaymentSuccess();

        // Reset to be ready for the next order
        InvoiceItems.Clear();

        _newInvoiceDto.StaffId = -1;
        _newInvoiceDto.StaffName = string.Empty;
        _newInvoiceDto.InvoiceItems.Clear();

        NewInvoiceId = await invoiceDao.GetNextInvoiceIdAsync(CurrentStore.Id);
        NewInvoiceCreatedAt = DateTime.MinValue;
        NewInvoiceItemCount = 0;
        NewInvoiceSubTotal = 0m;
        NewInvoiceTotalDiscount = 0m;
        NewInvoiceInvoiceBasedVAT = 0m;
        NewInvoiceTotal = 0m;
        NewInvoiceCustomerPaid = 0m;
        NewInvoiceChange = 0m;
        NewInvoicePaymentMethod = "CASH"; // default
    }

    public void NotifyPaymentSuccess()
    {
        if (NewInvoicePaymentMethod == "CASH")
        {
            textToSpeechService.SpeakPaymentSuccessViaCash(NewInvoiceTotal);
        }
        else if (NewInvoicePaymentMethod == "QR_PAY")
        {
            textToSpeechService.SpeakPaymentSuccessViaQRPay(NewInvoiceTotal);
        }
    }

    public async Task HandleCreateQrPaymentCodeButtonClick()
    {
        // Setup Payment link to use with PayOS
        List<ItemData> paymentItems = new();

        foreach (var invoiceItem in InvoiceItems)
        {
            var itemNameWithOption = $"{invoiceItem.ItemName} ({invoiceItem.OptionName})";

            paymentItems.Add(new(itemNameWithOption, 1, (int)invoiceItem.OptionPrice));
        }

        QrPayOrderCode = Int64.Parse(DateTime.Now.ToString("yyyyMMddHHmmss")); // Precision of Order Code: 1 second

        PaymentData paymentData = new
        (
            orderCode: QrPayOrderCode,
            amount: (int)NewInvoiceTotal,
            description: "Thanh toan don hang",
            items: paymentItems,
            cancelUrl: "",
            returnUrl: "",
            expiredAt: DateTimeOffset.UtcNow.AddSeconds(QRPAY_EXPIRE_DURATION_IN_SECONDS).ToUnixTimeSeconds()
        );

        CreatePaymentResult createPaymentResult = await payOsClient.createPaymentLink(paymentData);

        _qrCodeData = createPaymentResult.qrCode;
        QrPayAccountNumber = createPaymentResult.accountNumber;

        QrCode = await GenerateQrCodeAsync();
        QrPaySecondsRemaining = QRPAY_EXPIRE_DURATION_IN_SECONDS;
    }

    public async Task<bool> CheckQrPaymentCompleted()
    {
        if (QrPaySecondsRemaining <= 0)
        {
            // no QR-Pay payment at the moment

            return false;
        }

        if (QrPayOrderCode == 0)
            return false;

        PaymentLinkInformation paymentLinkInformation = await payOsClient.getPaymentLinkInformation(QrPayOrderCode);
    
        if (paymentLinkInformation.status == "PAID")
        {
            await handlePaymentComplete();

            return true;
        }

        return false;
    }

    private async Task handlePaymentComplete()
    {
        // Reset QR-Pay properties, be ready for next time
        QrPaySecondsRemaining = 0;
        QrPayOrderCode = 0;
        QrPayOrderCodeString = string.Empty;
        _qrCodeData = string.Empty;
        QrPayAccountNumber = string.Empty;
        QrCode = new(new Uri("ms-appx:///Assets/blank_qr.png"));

        // Handle invoice complete
        await ProceedWithPayment();
    }

    public void HandleQrPaymentTimeout()
    {
        // Cancel payment link (no need, it auto-expires)
        // await payOsClient.cancelPaymentLink(QrPayOrderCode);

        // Reset QR-Pay properties, be ready for next time
        // QrPaySecondsRemaining already reached 0
        QrPayOrderCode = 0;
        QrPayOrderCodeString = string.Empty;
        _qrCodeData = string.Empty;
        QrPayAccountNumber = string.Empty;
        QrCode = new(new Uri("ms-appx:///Assets/blank_qr.png"));

        // UI elements was updated in the UI thread
        // (check timer constructor of CashierMenuView)
    }

    public string ValidatePaymentMonetaryDetails()
    {
        if (NewInvoiceCustomerPaid % 500 != 0)
            return "Số tiền khách trả phải chia hết cho 500";

        if (NewInvoiceCustomerPaid < NewInvoiceTotal)
            return "Số tiền khách trả chưa đủ";

        if (NewInvoiceCustomerPaid - NewInvoiceTotal > 500_000m)
            return "Vui lòng kiểm tra lại số tiền khách trả";

        return "";
    }

    public void ResetPaymentMonetaryDetails()
    {
        NewInvoiceChange = 0m;
        NewInvoiceCustomerPaid = 0m;
    }

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
    {
        using MemoryStream memoryStream = new MemoryStream();
        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
        memoryStream.Seek(0, SeekOrigin.Begin);

        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.SetSource(memoryStream.AsRandomAccessStream());
        return bitmapImage;
    }

    private Task<BitmapImage> GenerateQrCodeAsync()
    {
        using QRCodeGenerator qrGenerator = new();
        using QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(_qrCodeData, QRCodeGenerator.ECCLevel.Q));
        
        Bitmap qrCodeImage = qrCode.GetGraphic(20);
        
        return Task.FromResult(ConvertBitmapToBitmapImage(qrCodeImage));
    }

    public List<Category> Categories
    {
        get => _categories;
        set
        {
            _categories = value;
            OnPropertyChanged(nameof(Categories));
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

    public TrulyObservableCollection<InvoiceItemDto> InvoiceItems
    {
        get => _invoiceItems;
        set
        {
            _invoiceItems = value;
            OnPropertyChanged(nameof(InvoiceItems));
        }
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

    public decimal NewInvoiceInvoiceBasedVAT
    {
        get => _newInvoiceDto.InvoiceBasedVAT;
        set
        {
            _newInvoiceDto.InvoiceBasedVAT = value;
            OnPropertyChanged(nameof(NewInvoiceInvoiceBasedVAT));
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

    public decimal NewInvoiceCustomerPaid
    {
        get => _newInvoiceDto.CustomerPaid;
        set
        {
            _newInvoiceDto.CustomerPaid = value;
            OnPropertyChanged(nameof(NewInvoiceCustomerPaid));
        }
    }

    public decimal NewInvoiceChange
    {
        get => _newInvoiceDto.Change;
        set
        {
            _newInvoiceDto.Change = value;
            OnPropertyChanged(nameof(NewInvoiceChange));
        }
    }

    public string NewInvoicePaymentMethod
    {
        get => _newInvoiceDto.PaymentMethod;
        set
        {
            _newInvoiceDto.PaymentMethod = value;
            OnPropertyChanged(nameof(NewInvoicePaymentMethod));
        }
    }

    public BitmapImage QrCode
    {
        get => _qrCode;
        set
        {
            _qrCode = value;
            OnPropertyChanged(nameof(QrCode));
        }
    }

    public long QrPaySecondsRemaining
    {
        get => _qrPaySecondsRemaining;
        set
        {
            _qrPaySecondsRemaining = value;
            OnPropertyChanged(nameof(QrPaySecondsRemaining));
        }
    }

    public long QrPayOrderCode
    {
        get => _qrPayOrderCode;
        set
        {
            _qrPayOrderCode = value;
            QrPayOrderCodeString = value.ToString();
            OnPropertyChanged(nameof(QrPayOrderCode));
        }
    }

    public string QrPayAccountNumber
    {
        get => _qrPayAccountNumber;
        set
        {
            _qrPayAccountNumber = value;
            OnPropertyChanged(nameof(QrPayAccountNumber));
        }
    }

    public string QrPayOrderCodeString
    {
        get => _qrPayOrderCodeString;
        set
        {
            _qrPayOrderCodeString = value;
            OnPropertyChanged(nameof(QrPayOrderCodeString));
        }
    }
}