using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a new invoice ID to a formatted invoice ID string and vice versa.
/// </summary>
class NewInvoiceIdToNewInvoiceIdStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a new invoice ID to a formatted invoice ID string.
    /// </summary>
    /// <param name="value">The new invoice ID to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the formatted invoice ID string if the value is a long; otherwise, returns an error message.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is long)
        {
            var newInvoiceId = (long)value;

            if (newInvoiceId == -1)
                return "";

            return "HD" + newInvoiceId.ToString("D6");
        }

        return "Lỗi chuyển định dạng";
    }

    /// <summary>
    /// Converts a formatted invoice ID string back to a new invoice ID.
    /// </summary>
    /// <param name="value">The formatted invoice ID string to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the new invoice ID if the value is a valid formatted invoice ID string; otherwise, returns -1.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var newInvoiceIdString = (string)value;

            if (newInvoiceIdString == "")
                return -1;

            // Removes the "HD" prefix
            newInvoiceIdString = newInvoiceIdString.Substring(2);

            return long.Parse(newInvoiceIdString);
        }

        return -1;
    }
}
