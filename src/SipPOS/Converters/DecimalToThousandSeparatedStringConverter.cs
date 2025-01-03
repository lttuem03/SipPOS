using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

public class DecimalToThousandSeparatedStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal)
        {
            var decimalValue = (decimal)value;
            var vietnameseFormatted = decimalValue.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));

            var thousandFormatted = vietnameseFormatted.Replace("₫", "");

            return thousandFormatted.Trim();
        }

        return "Lỗi chuyển định dạng";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var stringValue = (string)value;

            stringValue = stringValue.Replace(".", "");

            return decimal.Parse(stringValue);
        }

        return -1m;
    }
}