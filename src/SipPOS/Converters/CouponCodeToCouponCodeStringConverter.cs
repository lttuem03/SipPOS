using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a coupon code to a formatted coupon code string and vice versa.
/// </summary>
public class CouponCodeToCouponCodeStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a coupon code to a formatted coupon code string.
    /// </summary>
    /// <param name="value">The coupon code to convert.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the formatted coupon code string if the coupon code is not empty; otherwise, returns an empty string.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string couponCode)
        {
            if (couponCode == string.Empty)
                return string.Empty;

            return "(" + couponCode + ")";
        }

        return string.Empty;
    }

    /// <summary>
    /// Converts a formatted coupon code string back to a coupon code.
    /// </summary>
    /// <param name="value">The formatted coupon code string to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns the coupon code if the formatted coupon code string is valid; otherwise, returns an empty string.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string couponCodeString)
        {
            if (couponCodeString == string.Empty)
                return string.Empty;

            if (couponCodeString.StartsWith("(") && couponCodeString.EndsWith(")"))
                return couponCodeString.Substring(1, couponCodeString.Length - 2);
        }

        return string.Empty;
    }
}
