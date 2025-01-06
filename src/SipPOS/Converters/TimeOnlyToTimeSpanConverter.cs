using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

/// <summary>
/// Converts a TimeOnly value to a TimeSpan value.
/// </summary>
public class TimeOnlyToTimeSpanConverter : IValueConverter
{
    /// <summary>
    /// Converts a TimeOnly value to a TimeSpan value.
    /// </summary>
    /// <param name="value">The TimeOnly value to convert.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>A TimeSpan value representing the TimeOnly value.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is TimeOnly timeOnly)
        {
            return timeOnly.ToTimeSpan();
        }

        return TimeSpan.Zero;
    }

    /// <summary>
    /// This method is not implemented.
    /// </summary>
    /// <param name="value">The value to convert back.</param>
    /// <param name="targetType">The target type of the conversion.</param>
    /// <param name="parameter">An optional parameter for the conversion.</param>
    /// <param name="language">The language of the conversion.</param>
    /// <returns>Throws a NotImplementedException.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
