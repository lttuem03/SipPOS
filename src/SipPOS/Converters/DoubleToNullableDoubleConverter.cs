using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Converters;

public class DoubleToNullableDoubleConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is double nullableDouble ? nullableDouble : 0.0;
    }

    public object? ConvertBack(object value, Type targetType, object parameter, string language)
    {
        return value is double doubleValue ? (double?)doubleValue : null;
    }
}
