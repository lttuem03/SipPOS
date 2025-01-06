using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a product option tuple to a formatted string.
/// </summary>
public class ProductOptionToProductOptionStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a product option tuple to a formatted string.
    /// </summary>
    /// <param name="value">The product option tuple to convert.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A string representing the product option in the format "name (price₫)".</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is (string name, decimal price))
        {
            return $"{name} ({price:0,0}₫)";
        }

        return "Lỗi chuyển định dạng";
    }

    /// <summary>
    /// This method is not implemented.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
