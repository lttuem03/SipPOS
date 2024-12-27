using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

public class ProductOptionToPriceOnlyStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is (string name, decimal price))
        {
            return price.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        }

        return "Lỗi chuyển định dạng";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}