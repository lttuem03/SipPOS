using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Converters;

/// <summary>
/// Converts a double to a nullable double and vice versa.
/// </summary>
public class DoubleToNullableDoubleConverter : IValueConverter
{
    /// <summary>
    /// Converts a double to a nullable double.
    /// </summary>
    /// <param name="value">The double value to convert.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>The nullable double value if the input is a double; otherwise, 0.0.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is double nullableDouble ? nullableDouble : 0.0;
    }

    /// <summary>
    /// Converts a nullable double back to a double.
    /// </summary>
    /// <param name="value">The nullable double value to convert.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>The double value if the input is a nullable double; otherwise, null.</returns>
    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is double doubleValue ? (double?)doubleValue : null;
    }
}
