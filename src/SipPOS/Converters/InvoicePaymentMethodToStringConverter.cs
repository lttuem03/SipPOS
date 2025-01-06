using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a payment method code to its Vietnamese meaning string.
/// </summary>
public class InvoicePaymentMethodToStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a payment method code to its Vietnamese meaning string.
    /// </summary>
    /// <param name="value">The payment method code to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the Vietnamese meaning string if the value is a valid payment method code; otherwise, returns an error message.</returns>
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

    /// <summary>
    /// This method is not implemented.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
