using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

public class DecimalToVietnamDongFormattedStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is decimal)
        {
            var decimalValue = (decimal)value;

            return decimalValue.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
        }

        return "Lỗi chuyển định dạng";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var stringValue = (string)value;

            stringValue = stringValue.Replace(".", "");
            stringValue = stringValue.Substring(0, stringValue.Length - 2);

            return decimal.Parse(stringValue);
        }

        return -1m;
    }
}