using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

public class DateTimeToDateOnlyStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime)
        {
            return ((DateTime)value).ToString("dd/MM/yyyy");
        }

        return "Lỗi chuyển định dạng";
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return string.Empty;
    }
}
