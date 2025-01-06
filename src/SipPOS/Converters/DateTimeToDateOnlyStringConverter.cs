using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a DateTime object to a date-only string in the format "dd/MM/yyyy".
/// </summary>
public class DateTimeToDateOnlyStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a DateTime object to a date-only string.
    /// </summary>
    /// <param name="value">The DateTime object to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the date-only string in the format "dd/MM/yyyy" if the value is a DateTime object; otherwise, returns an error message.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime)
        {
            return ((DateTime)value).ToString("dd/MM/yyyy");
        }

        return "Lỗi chuyển định dạng";
    }

    /// <summary>
    /// Converts back a value to a DateTime object. This method is not implemented.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Always returns an empty string.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return string.Empty;
    }
}
