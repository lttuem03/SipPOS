using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;

public class TimeOnlyToTimeSpanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is TimeOnly timeOnly)
        {
            return timeOnly.ToTimeSpan();
        }

        return TimeSpan.Zero;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}