using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a double to a nullable double and vice versa.
/// </summary>
public class DecimalToDoubleConverter : IValueConverter
{
    // Convert decimal to double
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal decimalValue)
        {
            return (double)decimalValue;
        }
        return 0.0;
    }

    // Convert back from double to decimal
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is double doubleValue)
        {
            if (Double.IsNaN(doubleValue))
            {
                return 0m;
            }
            return (decimal)doubleValue;
        }
        return 0m;
    }
}
