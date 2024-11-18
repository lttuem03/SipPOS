using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SipPOS.Converters;

/// <summary>
/// Converts a value representing seconds remaining to a display string in the format "mm:ss".
/// </summary>
public class SecondsRemainingToDisplayConverter : IValueConverter
{
    /// <summary>
    /// Converts a value representing seconds remaining to a display string in the format "mm:ss".
    /// </summary>
    /// <param name="value">The value to convert, expected to be a long representing seconds remaining.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>A string representing the time in "mm:ss" format, or "00:00" if the value is not a valid long.</returns>
    object IValueConverter.Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is long secondsRemaining)
        {
            var timeSpan = TimeSpan.FromSeconds(secondsRemaining);
            return timeSpan.ToString(@"mm\:ss");
        }
        return "00:00";
    }

    /// <summary>
    /// This method is not implemented and will throw a <see cref="NotImplementedException"/> if called.
    /// </summary>
    /// <param name="value">The value that is to be converted back. This parameter is not used.</param>
    /// <param name="targetType">The type to convert to. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">Always thrown when this method is called.</exception>
    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
