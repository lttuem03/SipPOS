using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;
/// <summary>
/// Converts a DateTime object to a formatted date-time string and vice versa.
/// </summary>
public class DateTimeToDateTimeStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a DateTime object to a formatted date-time string.
    /// </summary>
    /// <param name="value">The DateTime object to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the formatted date-time string if the value is a DateTime object; otherwise, returns an error message.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime)
        {
            var dateTime = (DateTime)value;

            if (dateTime == DateTime.MinValue)
                return "";

            return dateTime.ToString("hh:mm tt dd/MM/yyyy");
        }

        return "Lỗi chuyển định dạng";
    }

    /// <summary>
    /// Converts a formatted date-time string back to a DateTime object.
    /// </summary>
    /// <param name="value">The formatted date-time string to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the DateTime object if the value is a valid formatted date-time string; otherwise, returns DateTime.MinValue.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var dateTimeString = (string)value;

            if (dateTimeString == "")
                return DateTime.MinValue;

            return DateTime.Parse(dateTimeString);
        }

        return DateTime.MinValue;
    }
}
