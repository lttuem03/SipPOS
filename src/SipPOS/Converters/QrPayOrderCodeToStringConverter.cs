using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

public class QrPayOrderCodeToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is long)
        {
            var orderCode = (long)value;

            if (orderCode == 0)
            {
                return "Đang tạo mã QR";
            }

            return orderCode;
        }

        return "0";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var orderCodeString = (string)value;

            if (orderCodeString == "Đang tạo mã QR")
                return (long)0;

            return long.Parse(orderCodeString);
        }

        return (long)0;
    }
}