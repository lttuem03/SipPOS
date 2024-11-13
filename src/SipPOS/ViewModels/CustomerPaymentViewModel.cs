using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

using SipPOS.Models;
using SipPOS.Services.Interfaces;
using QRCoder;
using Net.payOS.Types;
using System.Drawing;
using Microsoft.UI.Xaml.Media.Imaging;
using SipPOS.DataTransfer;
using Net.payOS;
using System.Threading;
using System.Diagnostics;
namespace SipPOS.ViewModels;

public partial class CustomerPaymentViewModel : ObservableRecipient
{
    public ObservableCollection<ProductDto> Products = new ObservableCollection<ProductDto>();

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

    private PayOS PayOS;

    public CustomerPaymentViewModel()
    {
        TotalPrice = 0;
        string ClientId = DotNetEnv.Env.GetString("PAYOS_CLIENT_ID");
        string ApiKey = DotNetEnv.Env.GetString("PAYOS_API_KEY");
        string ChecksumKey = DotNetEnv.Env.GetString("PAYOS_CHECKSUM_KEY");
        OrderCode = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
        PayOS = new PayOS(ClientId, ApiKey, ChecksumKey);
    }

    public async void CalculateTotalPrice()
    {
        TotalPrice = 0;
        foreach (var product in Products)
        {
            if (product.Price.HasValue && product.Quantity.HasValue)
            {
                TotalPrice += (int)(product.Price * product.Quantity);
            }
        }
        await GenerateQRCode();
        CountDown();
        CheckWasPayed();
    }

    public async Task GenerateQRCode()
    {
        SecondsRemaining = 5 * 60;
        await CreatePayment();
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        {
            using (QRCode qrCode = new QRCode(qrGenerator.CreateQrCode(QrCodeData, QRCodeGenerator.ECCLevel.Q)))
            {
                Bitmap qrCodeImage = qrCode.GetGraphic(20);
                QrCode = ConvertBitmapToBitmapImage(qrCodeImage);
                return;
            }
        }
        //handle error
    }

    private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
            memoryStream.Seek(0, SeekOrigin.Begin);

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.SetSource(memoryStream.AsRandomAccessStream());
            return bitmapImage;
        }
    }

    private async Task CreatePayment()
    {
        List<ItemData> items = GetItemDatas();
        long orderCode = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
        long expiredAt = DateTimeOffset.UtcNow.AddSeconds(SecondsRemaining).ToUnixTimeSeconds();
        PaymentData paymentData = new PaymentData(OrderCode, TotalPrice, "Thanh toan don hang", items, "", "", expiredAt: expiredAt);
        CreatePaymentResult createPayment = await PayOS.createPaymentLink(paymentData);
        QrCodeData = createPayment.qrCode;
        AccountNumber = createPayment.accountNumber;
    }

    private List<ItemData> GetItemDatas()
    {
        List<ItemData> itemDatas = new List<ItemData>();
        foreach (var product in Products)
        {
            if (product.Price.HasValue && product.Quantity.HasValue && product.Name != null)
            {
                itemDatas.Add(new ItemData(product.Name, (int)product.Quantity, (int)product.Price));
            }
        }
        return itemDatas;
    }

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

    public async Task CancelPayment()
    {
        await PayOS.cancelPaymentLink(OrderCode);
    }

    private void HandlePaymentComplete()
    {
        QrCode = new BitmapImage(new Uri("ms-appx:///Assets/Payed.png"));
        isPayed = true;
        SecondsRemaining = 0;
    }

    private async void HandlePaymentFailed()
    {
        await CancelPayment();
    }

}
