using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a DateTime object to a DateTimeOffset object and vice versa.
/// </summary>
public class DateTimeToDateTimeOffsetConverter : IValueConverter
{
    /// <summary>
    /// Converts a DateTime object to a DateTimeOffset object.
    /// </summary>
    /// <param name="value">The DateTime object to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the DateTimeOffset object if the value is a DateTime object; otherwise, returns DateTimeOffset.MinValue.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime dateTime)
        {
            return new DateTimeOffset(dateTime);
        }
        return DateTimeOffset.MinValue;
    }

    /// <summary>
    /// Converts a DateTimeOffset object back to a DateTime object.
    /// </summary>
    /// <param name="value">The DateTimeOffset object to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the DateTime object if the value is a DateTimeOffset object; otherwise, returns DateTime.MinValue.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.DateTime;
        }
        return DateTime.MinValue;
    }
}
