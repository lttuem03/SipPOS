using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;
public class ButtonAccessibilityToButtonOpacityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool isAccessible)
        {
            return isAccessible ? 1 : 0.5;
        }
        return 0.5;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}