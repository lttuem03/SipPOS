using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

namespace SipPOS.Converters;

/// <summary>
/// Converts a DateTime object to a formatted display string.
/// </summary>
public class DateToDisplayDateConverter : IValueConverter
{
    /// <summary>
    /// Converts a DateTime object to a formatted display string.
    /// </summary>
    /// <param name="value">The DateTime object to convert.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>A formatted date string if the value is a DateTime object; otherwise, the original value.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is DateTime)
        {
            return ((DateTime)value).ToString("dd/MM/yyyy HH:mm:ss");
        }
        return value;
    }

    /// <summary>
    /// Converts a formatted date string back to a DateTime object. This method is not implemented.
    /// </summary>
    /// <param name="value">The formatted date string.</param>
    /// <param name="targetType">The target type.</param>
    /// <param name="parameter">The converter parameter.</param>
    /// <param name="language">The language.</param>
    /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">Always thrown as this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotImplementedException();
    }
}
