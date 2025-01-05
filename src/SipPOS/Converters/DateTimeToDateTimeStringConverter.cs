using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;
public class DateTimeToDateTimeStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime)
        {
            var dateTime = (DateTime)value;

            if (dateTime == DateTime.MinValue)
                return "";

            return dateTime.ToString("hh:mm tt dd/MM/yyyy");
        }

        return "Lỗi chuyển định dạng";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string)
        {
            var dateTimeString = (string)value;

            if (dateTimeString == "")
                return DateTime.MinValue;

            return DateTime.Parse(dateTimeString);
        }

        return DateTime.MinValue;
    }
}