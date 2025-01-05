using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

public class InvoicePaymentMethodToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string paymentMethod)
        {
            var paymentMethodString = paymentMethod switch
            {
                "CASH" => "Tiền mặt",
                "QR_PAY" => "Quét QR-PAY",
                _ => "Không xác định"
            };

            return paymentMethodString;
        }

        return "Lỗi chuyển định dạng";    
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}