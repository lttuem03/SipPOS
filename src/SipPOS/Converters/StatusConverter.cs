using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Converters
{
    class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if ("Available".Equals(value))
            {
                return "Có sẵn";
            }
            else if ("Unavailable".Equals(value))
            {
                return "Không có sẵn";
            }
            return "Không xác định";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
