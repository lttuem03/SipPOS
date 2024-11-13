using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Converters
{
    public class SecondsRemainingToDisplayConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is long secondsRemaining)
            {
                var timeSpan = TimeSpan.FromSeconds(secondsRemaining);
                return timeSpan.ToString(@"mm\:ss");
            }
            return "00:00";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
