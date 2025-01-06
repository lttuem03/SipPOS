using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;
/// <summary>
/// Converts an item count to a formatted string.
/// </summary>
public class InvoiceItemCountToStringConverter : IValueConverter
{
    /// <summary>
    /// Converts an item count to a formatted string.
    /// </summary>
    /// <param name="value">The item count to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the formatted string if the value is a long; otherwise, returns an error message.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is long itemCount)
        {
            return itemCount.ToString() + " sản phẩm";
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
