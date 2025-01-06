using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a decimal value to a thousand-separated string and vice versa.
/// </summary>
public class DecimalToThousandSeparatedStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a decimal value to a thousand-separated string.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the thousand-separated string if the value is a decimal; otherwise, returns an error message.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal)
        {
            var decimalValue = (decimal)value;
            var vietnameseFormatted = decimalValue.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));

            var thousandFormatted = vietnameseFormatted.Replace("₫", "");

            return thousandFormatted.Trim();
        }

        return "Lỗi chuyển định dạng";
    }

    /// <summary>
    /// Converts a thousand-separated string back to a decimal value.
    /// </summary>
    /// <param name="value">The thousand-separated string to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the decimal value if the value is a valid thousand-separated string; otherwise, returns -1m.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var stringValue = (string)value;

            stringValue = stringValue.Replace(".", "");

            return decimal.Parse(stringValue);
        }

        return -1m;
    }
}
