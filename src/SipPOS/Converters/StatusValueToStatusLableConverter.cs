using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a status value to a status label.
/// </summary>
class StatusValueToStatusLableConverter : IValueConverter
{
    /// <summary>
    /// Converts a status value to a status label.
    /// </summary>
    /// <param name="value">The status value to convert.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>A status label based on the status value.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if ("Available".Equals(value))
        {
            return "Có sẵn";
        }
        else if ("Unavailable".Equals(value))
        {
            return "Không có sẵn";
        } else if ("Inactive".Equals(value))
        {
            return "Chưa áp dụng";
        }
        else if ("Active".Equals(value))
        {
            return "Áp dụng";
        }
        else if ("Expired".Equals(value))
        {
            return "Đã hết hạn";
        }
        else if ("Cancelled".Equals(value))
        {
            return "Đã hủy";
        }
        return "Không xác định";
    }

    /// <summary>
    /// Converts a status label back to a status value. This method is not implemented.
    /// </summary>
    /// <param name="value">The status label to convert.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
