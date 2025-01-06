using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a QR Pay order code to a string and vice versa.
/// </summary>
public class QrPayOrderCodeToStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a QR Pay order code to a string.
    /// </summary>
    /// <param name="value">The QR Pay order code to convert.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A string representing the QR Pay order code or a message if the code is 0.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is long)
        {
            var orderCode = (long)value;

            if (orderCode == 0)
            {
                return "Đang tạo mã QR";
            }

            return orderCode;
        }

        return "0";
    }

    /// <summary>
    /// Converts a string back to a QR Pay order code.
    /// </summary>
    /// <param name="value">The string to convert back.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>The QR Pay order code if the string is valid; otherwise, 0.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var orderCodeString = (string)value;

            if (orderCodeString == "Đang tạo mã QR")
                return (long)0;

            return long.Parse(orderCodeString);
        }

        return (long)0;
    }
}
