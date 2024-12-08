using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a price value to a display string with currency format.
/// </summary>
public class PriceToDisplayPriceConverter : IValueConverter
{
    /// <summary>
    /// Converts a price value to a display string with currency format.
    /// </summary>
    /// <param name="value">The price value to convert.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A string representing the price in currency format.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int)
        {
            return $"{(int)value:0,0} VND";
        }
        if (value is long)
        {
            return $"{(long)value:0,0} VND";
        }
        if (value is double)
        {
            return $"{(double)value:0,0} VND";
        }
        if (value is decimal)
        {
            return $"{(decimal)value:0,0} VND";
        }
        return value;
    }

    /// <summary>
    /// Converts a display string back to a price value.
    /// </summary>
    /// <param name="value">The display string to convert back.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>The original price value.</returns>
    /// <exception cref="NotImplementedException">Thrown when the method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
