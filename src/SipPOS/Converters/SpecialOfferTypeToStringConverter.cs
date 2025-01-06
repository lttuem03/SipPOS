using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a special offer type code to its Vietnamese meaning string.
/// </summary>
public class SpecialOfferTypeToStringConverter : IValueConverter
{
    /// <summary>
    /// Converts a special offer type code to its Vietnamese meaning string.
    /// </summary>
    /// <param name="value">The special offer type code to convert.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A string representing the special offer type or an error message if the code is not recognized.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var result = (string)value switch
            {
                "ProductPromotion" => "Sản phẩm",
                "CategoryPromotion" => "Danh mục",
                "InvoicePromotion" => "Hóa đơn",
                _ => "Không xác định"
            };

            return result;
        }

        return "Lỗi chuyển định dạng";
    }

    /// <summary>
    /// Converts a Vietnamese string back to a special offer type code.
    /// </summary>
    /// <param name="value">The string to convert back.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Returns "Không xác định" as the method is not implemented.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return "Không xác định";
    }
}
