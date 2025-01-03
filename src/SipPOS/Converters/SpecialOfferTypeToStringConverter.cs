using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

public class SpecialOfferTypeToStringConverter : IValueConverter
{
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

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return "Không xác định";
    }
}
