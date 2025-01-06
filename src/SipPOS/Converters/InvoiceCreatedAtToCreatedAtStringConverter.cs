using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;
/// <summary>
/// Converts a DateTime object representing the invoice creation time to a formatted string and vice versa.
/// </summary>
public class InvoiceCreatedAtToCreatedAtStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a DateTime object to a formatted string.
    /// </summary>
    /// <param name="value">The DateTime object to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the formatted string if the value is a DateTime object; otherwise, returns an error message.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime)
        {
            var createdAt = (DateTime)value;

            if (createdAt == DateTime.MinValue)
                return "";

            return createdAt.ToString("HH:mm dd/MM/yyyy");
        }

        return "Lỗi chuyển định dạng";
    }

    /// <summary>
    /// Converts a formatted string back to a DateTime object.
    /// </summary>
    /// <param name="value">The formatted string to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the DateTime object if the value is a valid formatted string; otherwise, returns DateTime.MinValue.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string dateString)
        {
            if (DateTime.TryParseExact(dateString, "HH:mm dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
        }

        return DateTime.MinValue;
    }
}
