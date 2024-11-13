using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Converters;


public class PriceToDisplayPriceConverter : IValueConverter
{

    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is int)
        {
            return $"{(int)value:0,0} VND";
        }
        if (value is long) {
            return $"{(long)value:0,0} VND";
        }
        if (value is double) {
            return $"{(double)value:0,0} VND";
        }
        if (value is decimal) {
            return $"{(decimal)value:0,0} VND";
        }
        return value;
    }


    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
