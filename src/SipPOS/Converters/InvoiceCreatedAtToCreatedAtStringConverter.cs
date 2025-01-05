using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;
public class InvoiceCreatedAtToCreatedAtStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime)
        {
            var createdAt = (DateTime)value;

            if (createdAt == DateTime.MinValue)
                return "";

            return createdAt.ToString("HH:mm dd/MM/yyyy");
        }

        return "Lỗi chuyển định dạng";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string dateString)
        {
            if (DateTime.TryParseExact(dateString, "HH:mm dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
        }

        return DateTime.MinValue;
    }
}