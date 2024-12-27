using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

public class ProductOptionToProductOptionStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is (string name, decimal price))
        {
            return $"{name} ({price:0,0}₫)";
        }

        return "Lỗi chuyển định dạng";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}