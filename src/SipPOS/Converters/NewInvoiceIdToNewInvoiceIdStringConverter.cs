using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

class NewInvoiceIdToNewInvoiceIdStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is long)
        {
            var newInvoiceId = (long)value;

            if (newInvoiceId == -1)
                return "Đang chờ tạo";

            return newInvoiceId.ToString("D6");
        }

        return "Lỗi chuyển định dạng";    
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var newInvoiceIdString = (string)value;

            if (newInvoiceIdString == "Đang chờ tạo")
                return -1;

            return long.Parse(newInvoiceIdString);
        }

        return -1;
    }
}