using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a nullable decimal to a double and vice versa.
/// </summary>
public class NullableDecimalToDoubleConverter : IValueConverter
{
    /// <summary>
    /// Converts a nullable decimal to a double.
    /// </summary>
    /// <param name="value">The nullable decimal value to convert.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>The double value if the input is a nullable decimal; otherwise, 0.0.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is decimal nullableDecimal ? (double)nullableDecimal : 0.0;
    }

    /// <summary>
    /// Converts a double back to a nullable decimal.
    /// </summary>
    /// <param name="value">The double value to convert.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>The nullable decimal value if the input is a double; otherwise, null.</returns>
    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is double doubleValue ? (decimal?)doubleValue : null;
    }
}
