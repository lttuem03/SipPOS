using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a decimal to a double and vice versa.
/// </summary>
public class DecimalToDoubleConverter : IValueConverter
{
    /// <summary>
    /// Converts a decimal value to a double value.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the double value if the value is a decimal; otherwise, returns 0.0.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal decimalValue)
        {
            return (double)decimalValue;
        }
        return 0.0;
    }

    /// <summary>
    /// Converts a double value back to a decimal value.
    /// </summary>
    /// <param name="value">The double value to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the decimal value if the value is a double; otherwise, returns 0m.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is double doubleValue)
        {
            if (double.IsNaN(doubleValue))
            {
                return 0m;
            }
            return (decimal)doubleValue;
        }
        return 0m;
    }
}
