using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

public class CouponCodeToCouponCodeStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string couponCode)
        {
            if (couponCode == string.Empty)
                return string.Empty;

            return "(" + couponCode + ")";
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is string couponCodeString)
        {
            if (couponCodeString == string.Empty)
                return string.Empty;

            if (couponCodeString.StartsWith("(") && couponCodeString.EndsWith(")"))
                return couponCodeString.Substring(1, couponCodeString.Length - 2);
        }

        return string.Empty;
    }
}
