using System.Drawing;
using System.Collections.ObjectModel;

using Microsoft.UI.Xaml.Media.Imaging;

using CommunityToolkit.Mvvm.ComponentModel;

using QRCoder;
using Net.payOS;
using Net.payOS.Types;

using SipPOS.DataTransfer.Entity;

namespace SipPOS.ViewModels.Cashier;

/// <summary>
/// ViewModel for handling customer payments.
/// </summary>
public partial class CustomerPaymentViewModel : ObservableRecipient
{
    public ObservableCollection<ProductDto> Products = new();

    [ObservableProperty]
    private int totalPrice = 0;

    [ObservableProperty]
    private BitmapImage? qrCode;

    [ObservableProperty]
    private string accountNumber = "";

    [ObservableProperty]
    private long secondsRemaining;

    [ObservableProperty]
    private long orderCode;

    private string QrCodeData = "";

    private bool isPayed = false;

    private readonly PayOS PayOS;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerPaymentViewModel"/> class.
    /// </summary>
    public CustomerPaymentViewModel()
    {
        TotalPrice = 0;
        var ClientId = DotNetEnv.Env.GetString("PAYOS_CLIENT_ID");
        var ApiKey = DotNetEnv.Env.GetString("PAYOS_API_KEY");
        var ChecksumKey = DotNetEnv.Env.GetString("PAYOS_CHECKSUM_KEY");
        OrderCode = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
        PayOS = new PayOS(ClientId, ApiKey, ChecksumKey);
    }

    /// <summary>
    /// Calculates the total price of the products and generates a QR code for payment.
    /// </summary>
    public async void CalculateTotalPrice()
    {
        TotalPrice = 0;
        //foreach (var product in Products)
        //{
        //    if (product.Price.HasValue && product.Quantity.HasValue)
        //    {
        //        TotalPrice += (int)(product.Price * product.Quantity);
        //    }
        //}
        await GenerateQRCode();
        CountDown();
        CheckWasPayed();
    }

    /// <summary>
    /// Generates a QR code for the payment.
    /// </summary>
    public async Task GenerateQRCode()
    {
        SecondsRemaining = 5 * 60;
        await CreatePayment();
        using QRCodeGenerator qrGenerator = new QRCodeGenerator();
        using QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(QrCodeData, QRCodeGenerator.ECCLevel.Q));
        Bitmap qrCodeImage = qrCode.GetGraphic(20);
        QrCode = ConvertBitmapToBitmapImage(qrCodeImage);
        return;
        //handle error
    }

    /// <summary>
    /// Converts a Bitmap to a BitmapImage.
    /// </summary>
    /// <param name="bitmap">The bitmap to convert.</param>
    /// <returns>The converted BitmapImage.</returns>
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
    /// Creates a payment link and sets the QR code data and account number.
    /// </summary>
    private async Task CreatePayment()
    {
        List<ItemData> items = GetItemDatas();
        var expiredAt = DateTimeOffset.UtcNow.AddSeconds(SecondsRemaining).ToUnixTimeSeconds();
        PaymentData paymentData = new PaymentData(OrderCode, TotalPrice, "Thanh toan don hang", items, "", "", expiredAt: expiredAt);
        CreatePaymentResult createPayment = await PayOS.createPaymentLink(paymentData);
        QrCodeData = createPayment.qrCode;
        AccountNumber = createPayment.accountNumber;
    }

    /// <summary>
    /// Gets the item data from the products.
    /// </summary>
    /// <returns>A list of item data.</returns>
    private List<ItemData> GetItemDatas()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        //foreach (var product in Products)
        //{
        //    if (product.Price.HasValue && product.Quantity.HasValue && product.Name != null)
        //    {
        //        itemDatas.Add(new ItemData(product.Name, (int)product.Quantity, (int)product.Price));
        //    }
        //}
        return itemDatas;
    }

    /// <summary>
    /// Checks if the payment was completed.
    /// </summary>
    private async void CheckWasPayed()
    {
        while (true)
        {
            PaymentLinkInformation paymentLinkInformation = await PayOS.getPaymentLinkInformation(OrderCode);
            if (paymentLinkInformation.status == "PAID")
            {
                HandlePaymentComplete();
                break;
            }
            await Task.Delay(4000);
        }
    }

    /// <summary>
    /// Starts the countdown for the payment expiration.
    /// </summary>
    public async void CountDown()
    {
        while (true)
        {
            SecondsRemaining -= 1;
            if (SecondsRemaining <= 0)
            {
                if (!isPayed)
                {
                    HandlePaymentFailed();
                }
                break;
            }
            await Task.Delay(1000);
        }
    }

    /// <summary>
    /// Cancels the payment.
    /// </summary>
    public async Task CancelPayment()
    {
        await PayOS.cancelPaymentLink(OrderCode);
    }

    /// <summary>
    /// Handles the payment completion.
    /// </summary>
    private void HandlePaymentComplete()
    {
        QrCode = new BitmapImage(new Uri("ms-appx:///Assets/Payed.png"));
        isPayed = true;
        SecondsRemaining = 0;
    }

    /// <summary>
    /// Handles the payment failure.
    /// </summary>
    private async void HandlePaymentFailed()
    {
        await CancelPayment();
    }
}
