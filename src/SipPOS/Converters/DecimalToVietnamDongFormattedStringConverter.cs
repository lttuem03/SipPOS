using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a decimal value to a formatted Vietnamese Dong string and vice versa.
/// </summary>
public class DecimalToVietnamDongFormattedStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a decimal value to a formatted Vietnamese Dong string.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the formatted Vietnamese Dong string if the value is a decimal; otherwise, returns an error message.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal)
        {
            var decimalValue = (decimal)value;

            return decimalValue.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        }

        return "Lỗi chuyển định dạng";
    }

    /// <summary>
    /// Converts a formatted Vietnamese Dong string back to a decimal value.
    /// </summary>
    /// <param name="value">The formatted Vietnamese Dong string to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the decimal value if the value is a valid formatted Vietnamese Dong string; otherwise, returns -1m.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var stringValue = (string)value;

            stringValue = stringValue.Replace(".", "");
            stringValue = stringValue.Substring(0, stringValue.Length - 2);

            return decimal.Parse(stringValue);
        }

        return -1m;
    }
}
