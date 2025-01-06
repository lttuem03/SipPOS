using System.Drawing;
using System.ComponentModel;

using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Controls;

using SipPOS.Resources.Helper;
using SipPOS.Views.Login;
using SipPOS.Models.Entity;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.Entity.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Context.Configuration.Interfaces;

using QRCoder;
using Net.payOS;
using Net.payOS.Types;

namespace SipPOS.ViewModels.Cashier;

/// <summary>
/// ViewModel for the cashier menu, handling various operations such as item search, category browsing, order management, and payment processing.
/// </summary>
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

    private TrulyObservableCollection<SpecialOffer> _specialOffers = new();
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

    private readonly ITextToSpeechService textToSpeechService;

    private readonly PayOS payOsClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="CashierMenuViewModel"/> class.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the store authentication service, staff authentication service, or configuration is not registered or loaded,
    /// or when the store or staff is not logged in.
    /// </exception>
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

    /// <summary>
    /// Asynchronously initializes the ViewModel by loading configuration, categories, products, and special offers.
    /// </summary>
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

        // Load special offers
        var specialOfferDao = App.GetService<ISpecialOfferDao>();
        var specialOffers = await specialOfferDao.GetAllAsync(storeId);

        foreach (var specialOffer in specialOffers)
        {
            // status is "Active"
            if (specialOffer.Status != "Active")
                continue;

            // still has times of usage
            if (specialOffer.ItemsSold < specialOffer.MaxItems)
                _specialOffers.Add(specialOffer);
        }

        // Initial "All categories" is chosen
        ProductsOnDisplay = _allProducts;

        // Initiate new invoice
        _newInvoiceDto.Id = await App.GetService<IInvoiceDao>().GetNextInvoiceIdAsync(storeId);
    }

    /// <summary>
    /// Handles the text changed event of the item search TextBox.
    /// </summary>
    /// <param name="keywords">The search keywords.</param>
    public void HandleItemSearchTextBoxTextChanged(string keywords)
    {
        ProductsOnDisplay = _allProducts.Where(p => p.Name.Contains(keywords)).ToList();
    }

    /// <summary>
    /// Handles the selection changed event of the category browsing GridView.
    /// </summary>
    /// <param name="selectedCategory">The selected category.</param>
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

    /// <summary>
    /// Handles the add item to order button click event.
    /// </summary>
    /// <param name="productItem">The product item to add.</param>
    /// <param name="couponNotApplicableWarningTextBlock">The TextBlock to display a warning if the coupon is not applicable.</param>
    /// <returns>The added invoice item.</returns>
    public InvoiceItemDto HandleAddItemToOrderButtonClick(Product productItem, TextBlock couponNotApplicableWarningTextBlock)
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

        // Check if there was any ProductPromotion
        // coupon applied to the product of this invoice item
        if (NewInvoiceCouponCode != string.Empty && NewInvoiceTotalDiscount == 0m)
        {
            var specialOffer = SpecialOffers.First(sp => sp.Code == NewInvoiceCouponCode);

            if (specialOffer.Type == "ProductPromotion")
            {
                if (invoiceItemDto.ProductId == specialOffer.ProductId)
                {
                    switch (specialOffer.PriceType)
                    {
                        case "Original":
                            invoiceItemDto.Discount = specialOffer.DiscountPrice ?? 0m;
                            break;
                        case "Percentage":
                            invoiceItemDto.Discount = invoiceItemDto.OptionPrice * (specialOffer.DiscountPercentage ?? 0m) * 0.01m;
                            break;
                    }

                    NewInvoiceTotalDiscount = invoiceItemDto.Discount;

                    // Un-show the warning
                    couponNotApplicableWarningTextBlock.Opacity = 0F; 
                }
            }
        }

        InvoiceItems.Add(invoiceItemDto);

        // Update payment details
        NewInvoiceItemCount = InvoiceItems.Count;
        NewInvoiceSubTotal = InvoiceItems.Sum(orderItem => orderItem.OptionPrice);
        //NewInvoiceTotalDiscount = InvoiceItems.Sum(orderItem => orderItem.Discount); // removed cuz discount is handled when coupon is applied

        NewInvoiceInvoiceBasedVAT = CurrentConfiguration.VatMethod switch
        {
            "ORDER_BASED" => NewInvoiceSubTotal * 0.05m,
            _ => 0m
        };

        NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceInvoiceBasedVAT;

        return invoiceItemDto;
    }

    /// <summary>
    /// Handles the remove item from order button click event.
    /// </summary>
    /// <param name="invoiceItemDto">The invoice item to remove.</param>
    /// <param name="couponNotApplicableWarningTextBlock">The TextBlock to display a warning if the coupon is not applicable.</param>
    public void HandleRemoveItemFromOrderButtonClick(InvoiceItemDto invoiceItemDto, TextBlock couponNotApplicableWarningTextBlock)
    {
        long? specialOfferProductId = null;
        decimal? specialOfferDiscountValue = null;

        // Check if any coupon was applied on this InvoiceItem
        if (invoiceItemDto.Discount != 0m)
        {
            // if it has, before we remove it,
            // we save its product id and discount
            // value, and then we can check for any
            // other item that is of the same product
            // (In case the coupon applied was of
            // ProductPromotion type)

            // Find which special offer was applied
            var specialOffer = SpecialOffers.First(sp => sp.Code == NewInvoiceCouponCode);

            if (specialOffer.Type == "ProductPromotion")
            {
                specialOfferProductId = invoiceItemDto.ProductId;
                specialOfferDiscountValue = invoiceItemDto.Discount;
            }
        }

        InvoiceItems.Remove(invoiceItemDto);
        _newInvoiceDto.InvoiceItems.Remove(invoiceItemDto);

        // Update the UI
        foreach (var invoiceItem in InvoiceItems)
        {
            invoiceItem.Ordinal = InvoiceItems.IndexOf(invoiceItem) + 1;

            if (specialOfferProductId != null)
            {
                if (invoiceItem.ProductId == specialOfferProductId)
                {
                    invoiceItem.Discount = specialOfferDiscountValue ?? 0m;

                    specialOfferProductId = null;
                    specialOfferDiscountValue = null;
                }
            }
        }

        // Update payment details
        NewInvoiceItemCount = InvoiceItems.Count;
        NewInvoiceSubTotal = InvoiceItems.Sum(invoiceItem => invoiceItem.OptionPrice);
        //NewInvoiceTotalDiscount = InvoiceItems.Sum(orderItem => orderItem.Discount); // removed cuz discount is handled when coupon is applied

        NewInvoiceInvoiceBasedVAT = CurrentConfiguration.VatMethod switch
        {
            "ORDER_BASED" => NewInvoiceSubTotal * 0.05m,
            _ => 0m
        };

        NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceInvoiceBasedVAT;

        if (InvoiceItems.Count == 0)
        {
            NewInvoiceCreatedAt = DateTime.MinValue;
            NewInvoiceCouponCode = string.Empty;

            return;
        }

        // If there is still invoice items left,
        // but the previously applied ProductPromotion
        // coupon was never re-applied to any other
        // item, we show warning to the cashier
        if (specialOfferProductId != null && specialOfferDiscountValue != null)
        {
            NewInvoiceTotalDiscount = 0m;
            NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceInvoiceBasedVAT;

            couponNotApplicableWarningTextBlock.Opacity = 1F;
        }
    }

    /// <summary>
    /// Handles the cancel order button click event.
    /// </summary>
    /// <param name="cancelOrderConfimationContentDialog">The content dialog to confirm order cancellation.</param>
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

    /// <summary>
    /// Handles the selection changed event of the payment method radio buttons.
    /// </summary>
    /// <param name="selectedIndex">The index of the selected payment method.</param>
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

    /// <summary>
    /// Handles the add amount button click event on the numpad.
    /// </summary>
    /// <param name="amount">The amount to add.</param>
    public void HandleNumpadAddAmountButtonClick(decimal amount)
    {
        NewInvoiceCustomerPaid += amount;
        NewInvoiceChange = Math.Max(0, NewInvoiceCustomerPaid - NewInvoiceTotal);
    }

    /// <summary>
    /// Handles the number button click event on the numpad.
    /// </summary>
    /// <param name="numberText">The text of the number button clicked.</param>
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

    /// <summary>
    /// Handles the clear amount button click event on the numpad.
    /// </summary>
    public void HandleNumpadClearAmountButtonClick()
    {
        ResetPaymentMonetaryDetails();
    }

    /// <summary>
    /// Handles the backspace button click event on the numpad.
    /// </summary>
    public void HandleNumpadBackspaceButtonClick()
    {
        NewInvoiceCustomerPaid = Math.Floor(NewInvoiceCustomerPaid / 10);
        NewInvoiceChange = Math.Max(0, NewInvoiceCustomerPaid - NewInvoiceTotal);
    }

    /// <summary>
    /// Proceeds with the payment process.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the invoice insertion fails.</exception>
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

        // Update the coupon's # of usage (if one was applied)
        if (NewInvoiceCouponCode != string.Empty && NewInvoiceTotalDiscount != 0m)
        {
            var specialOffer = SpecialOffers.First(sp => sp.Code == NewInvoiceCouponCode);

            var specialOfferDao = App.GetService<ISpecialOfferDao>();
            var specialOfferOriginal = await specialOfferDao.GetByIdAsync(CurrentStore.Id, specialOffer.Id);

            if (specialOfferOriginal != null)
            {
                specialOfferOriginal.ItemsSold++;
                specialOfferOriginal.UpdatedAt = DateTime.Now;
                specialOfferOriginal.UpdatedBy = CurrentStaff.CompositeUsername;

                var specialOfferUpdated = await specialOfferDao.UpdateByIdAsync(CurrentStore.Id, specialOfferOriginal);

                if (specialOfferUpdated == null)
                {
                    // DO NOT THROW EXCEPTION, SAME REASON AS ABOVE
                }
                else
                {
                    specialOffer.ItemsSold = specialOfferUpdated.ItemsSold;
                }
            }
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
        NewInvoiceCouponCode = string.Empty;
    }

    /// <summary>
    /// Notifies the user of a successful payment.
    /// </summary>
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

    /// <summary>
    /// Handles the creation of a QR payment code.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Checks if the QR payment has been completed.
    /// </summary>
    /// <returns>A task representing the asynchronous operation, with a boolean result indicating whether the payment is completed.</returns>
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

    /// <summary>
    /// Handles the completion of a payment.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
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

    /// <summary>
    /// Handles the timeout of a QR payment.
    /// </summary>
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

    /// <summary>
    /// Validates the payment monetary details.
    /// </summary>
    /// <returns>A string containing the validation message, or an empty string if validation passes.</returns>
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

    /// <summary>
    /// Resets the payment monetary details.
    /// </summary>
    public void ResetPaymentMonetaryDetails()
    {
        NewInvoiceChange = 0m;
        NewInvoiceCustomerPaid = 0m;
    }

    /// <summary>
    /// Handles the application of a coupon when the apply coupon button is clicked.
    /// </summary>
    /// <param name="specialOffer">The special offer to apply.</param>
    /// <returns>True if the coupon was successfully applied, otherwise false.</returns>
    public bool HandleApplyCouponButtonClick(SpecialOffer specialOffer)
    {
        switch (specialOffer.Type)
        {
            case "ProductPromotion":
                // Search in the current order item list,
                // for any product that matches the special offer
                // product id (ONLY ONE IS APPLICABLE)
                foreach (var invoiceItem in InvoiceItems)
                {
                    if (invoiceItem.ProductId == specialOffer.ProductId)
                    {
                        // Apply the discount
                        switch (specialOffer.PriceType)
                        {
                            case "Original":
                                invoiceItem.Discount = specialOffer.DiscountPrice ?? 0m;
                                break;
                            case "Percentage":
                                invoiceItem.Discount = invoiceItem.OptionPrice * (specialOffer.DiscountPercentage ?? 0m) * 0.01m;
                                break;
                        }

                        // Update new invoice
                        NewInvoiceTotalDiscount = invoiceItem.Discount;
                        NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceInvoiceBasedVAT;

                        // Only 1 coupon per order
                        return true;
                    }
                }

                return false;

            case "CategoryPromotion":
                // Search in the current order item list,
                // for any product that matches the special offer
                // category id (ANY NUMBER OF PRODUCTS CAN BE APPLICABLE)

                var couponApplied = false;

                foreach (var invoiceItem in InvoiceItems)
                {
                    // Kind of tedious to do it this way
                    var product = _allProducts.First(p => p.Id == invoiceItem.ProductId);

                    if (product.CategoryId == specialOffer.CategoryId)
                    {
                        // Apply the discount
                        switch (specialOffer.PriceType)
                        {
                            case "Original":
                                invoiceItem.Discount = specialOffer.DiscountPrice ?? 0m;
                                break;
                            case "Percentage":
                                invoiceItem.Discount = invoiceItem.OptionPrice * (specialOffer.DiscountPercentage ?? 0m) * 0.01m;
                                break;
                        }

                        // Update new invoice
                        NewInvoiceTotalDiscount += invoiceItem.Discount;
                        NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceInvoiceBasedVAT;

                        couponApplied = true;
                    }
                }

                return couponApplied;

            case "InvoicePromotion":
                // update every invoice item's discount to 0m
                foreach (var invoiceItem in InvoiceItems)
                    invoiceItem.Discount = 0m;

                switch (specialOffer.PriceType)
                {
                    case "Original":
                        NewInvoiceTotalDiscount = specialOffer.DiscountPrice ?? 0m;
                        break;
                    case "Percentage":
                        NewInvoiceTotalDiscount = NewInvoiceSubTotal * (specialOffer.DiscountPercentage ?? 0m) * 0.01m;
                        break;
                }

                NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceInvoiceBasedVAT;
                return true;
        }

        return false;
    }

    /// <summary>
    /// Handles the application of a hidden coupon.
    /// </summary>
    /// <param name="hiddenCoupon">The hidden coupon code.</param>
    /// <returns>True if the hidden coupon was successfully applied, otherwise false.</returns>
    public bool HandleApplyHiddenCoupon(string hiddenCoupon)
    {
        // The hidden coupon is not implemented yet
        // so at this point the cashier can only choose from the list
        // of coupons

        // Reset discount information
        NewInvoiceTotalDiscount = 0m;

        foreach (var invoiceItem in InvoiceItems)
        {
            invoiceItem.Discount = 0m;
        }

        NewInvoiceTotal = NewInvoiceSubTotal - NewInvoiceTotalDiscount + NewInvoiceInvoiceBasedVAT;

        return false;
    }

    /// <summary>
    /// Handles the change of staff ID button click event.
    /// </summary>
    public void HandleChangeStaffIdButtonClick()
    {
        if (App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
            return;

        staffAuthenticationService.Logout();

        // Store is logged in, so return to LoginView
        // will change the login tab to StaffLogin
        App.NavigateTo(typeof(LoginView));
    }

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Converts a Bitmap to a BitmapImage.
    /// </summary>
    /// <param name="bitmap">The Bitmap to convert.</param>
    /// <returns>A BitmapImage created from the Bitmap.</returns>
    private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
    {
        using MemoryStream memoryStream = new MemoryStream();
        bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
        memoryStream.Seek(0, SeekOrigin.Begin);

        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.SetSource(memoryStream.AsRandomAccessStream());
        return bitmapImage;
    }

    /// <summary>
    /// Generates a QR code asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the generated QR code as a BitmapImage.</returns>
    private Task<BitmapImage> GenerateQrCodeAsync()
    {
        using QRCodeGenerator qrGenerator = new();
        using QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(_qrCodeData, QRCodeGenerator.ECCLevel.Q));

        Bitmap qrCodeImage = qrCode.GetGraphic(20);

        return Task.FromResult(ConvertBitmapToBitmapImage(qrCodeImage));
    }

    /// <summary>
    /// Gets or sets the list of categories.
    /// </summary>
    public List<Category> Categories
    {
        get => _categories;
        set
        {
            _categories = value;
            OnPropertyChanged(nameof(Categories));
        }
    }

    /// <summary>
    /// Gets or sets the list of products on display.
    /// </summary>
    public List<Product> ProductsOnDisplay
    {
        get => _productsOnDisplay;
        set
        {
            _productsOnDisplay = value;
            OnPropertyChanged(nameof(ProductsOnDisplay));
        }
    }

    /// <summary>
    /// Gets or sets the collection of special offers.
    /// </summary>
    public TrulyObservableCollection<SpecialOffer> SpecialOffers
    {
        get => _specialOffers;
        set
        {
            _specialOffers = value;
            OnPropertyChanged(nameof(SpecialOffers));
        }
    }

    /// <summary>
    /// Gets or sets the collection of invoice items.
    /// </summary>
    public TrulyObservableCollection<InvoiceItemDto> InvoiceItems
    {
        get => _invoiceItems;
        set
        {
            _invoiceItems = value;
            OnPropertyChanged(nameof(InvoiceItems));
        }
    }

    /// <summary>
    /// Gets or sets the VAT rate string.
    /// </summary>
    public string VatRateString
    {
        get => _vatRateString;
        set
        {
            _vatRateString = value;
            OnPropertyChanged(nameof(VatRateString));
        }
    }

    /// <summary>
    /// Gets or sets the VAT message string.
    /// </summary>
    public string VatMessageString
    {
        get => _vatMessageString;
        set
        {
            _vatMessageString = value;
            OnPropertyChanged(nameof(VatMessageString));
        }
    }

    /// <summary>
    /// Gets or sets the new invoice ID.
    /// </summary>
    public long NewInvoiceId
    {
        get => _newInvoiceDto.Id;
        set
        {
            _newInvoiceDto.Id = value;
            OnPropertyChanged(nameof(NewInvoiceId));
        }
    }

    /// <summary>
    /// Gets or sets the creation date and time of the new invoice.
    /// </summary>
    public DateTime NewInvoiceCreatedAt
    {
        get => _newInvoiceDto.CreatedAt;
        set
        {
            _newInvoiceDto.CreatedAt = value;
            OnPropertyChanged(nameof(NewInvoiceCreatedAt));
        }
    }

    /// <summary>
    /// Gets or sets the item count of the new invoice.
    /// </summary>
    public long NewInvoiceItemCount
    {
        get => _newInvoiceDto.ItemCount;
        set
        {
            _newInvoiceDto.ItemCount = value;
            OnPropertyChanged(nameof(NewInvoiceItemCount));
        }
    }

    /// <summary>
    /// Gets or sets the subtotal of the new invoice.
    /// </summary>
    public decimal NewInvoiceSubTotal
    {
        get => _newInvoiceDto.SubTotal;
        set
        {
            _newInvoiceDto.SubTotal = value;
            OnPropertyChanged(nameof(NewInvoiceSubTotal));
        }
    }

    /// <summary>
    /// Gets or sets the total discount of the new invoice.
    /// </summary>
    public decimal NewInvoiceTotalDiscount
    {
        get => _newInvoiceDto.TotalDiscount;
        set
        {
            _newInvoiceDto.TotalDiscount = value;
            OnPropertyChanged(nameof(NewInvoiceTotalDiscount));
        }
    }

    /// <summary>
    /// Gets or sets the invoice-based VAT for the new invoice.
    /// </summary>
    public decimal NewInvoiceInvoiceBasedVAT
    {
        get => _newInvoiceDto.InvoiceBasedVAT;
        set
        {
            _newInvoiceDto.InvoiceBasedVAT = value;
            OnPropertyChanged(nameof(NewInvoiceInvoiceBasedVAT));
        }
    }

    /// <summary>
    /// Gets or sets the total amount for the new invoice.
    /// </summary>
    public decimal NewInvoiceTotal
    {
        get => _newInvoiceDto.Total;
        set
        {
            _newInvoiceDto.Total = value;
            OnPropertyChanged(nameof(NewInvoiceTotal));
        }
    }

    /// <summary>
    /// Gets or sets the amount paid by the customer for the new invoice.
    /// </summary>
    public decimal NewInvoiceCustomerPaid
    {
        get => _newInvoiceDto.CustomerPaid;
        set
        {
            _newInvoiceDto.CustomerPaid = value;
            OnPropertyChanged(nameof(NewInvoiceCustomerPaid));
        }
    }

    /// <summary>
    /// Gets or sets the change to be given to the customer for the new invoice.
    /// </summary>
    public decimal NewInvoiceChange
    {
        get => _newInvoiceDto.Change;
        set
        {
            _newInvoiceDto.Change = value;
            OnPropertyChanged(nameof(NewInvoiceChange));
        }
    }

    /// <summary>
    /// Gets or sets the coupon code applied to the new invoice.
    /// </summary>
    public string NewInvoiceCouponCode
    {
        get => _newInvoiceDto.CouponCode;
        set
        {
            _newInvoiceDto.CouponCode = value;
            OnPropertyChanged(nameof(NewInvoiceCouponCode));
        }
    }

    /// <summary>
    /// Gets or sets the payment method used for the new invoice.
    /// </summary>
    public string NewInvoicePaymentMethod
    {
        get => _newInvoiceDto.PaymentMethod;
        set
        {
            _newInvoiceDto.PaymentMethod = value;
            OnPropertyChanged(nameof(NewInvoicePaymentMethod));
        }
    }

    /// <summary>
    /// Gets or sets the QR code image.
    /// </summary>
    public BitmapImage QrCode
    {
        get => _qrCode;
        set
        {
            _qrCode = value;
            OnPropertyChanged(nameof(QrCode));
        }
    }

    /// <summary>
    /// Gets or sets the remaining seconds for QR payment expiration.
    /// </summary>
    public long QrPaySecondsRemaining
    {
        get => _qrPaySecondsRemaining;
        set
        {
            _qrPaySecondsRemaining = value;
            OnPropertyChanged(nameof(QrPaySecondsRemaining));
        }
    }

    /// <summary>
    /// Gets or sets the QR payment order code.
    /// </summary>
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

    /// <summary>
    /// Gets or sets the QR payment account number.
    /// </summary>
    public string QrPayAccountNumber
    {
        get => _qrPayAccountNumber;
        set
        {
            _qrPayAccountNumber = value;
            OnPropertyChanged(nameof(QrPayAccountNumber));
        }
    }

    /// <summary>
    /// Gets or sets the QR payment order code as a string.
    /// </summary>
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