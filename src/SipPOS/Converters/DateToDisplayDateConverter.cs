using AutoMapper;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Converters;

public class DateToDisplayDateConverter: IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime)
        {
            return ((DateTime)value).ToString("dd/MM/yyyy HH:mm:ss");
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
