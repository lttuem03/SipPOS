using Microsoft.UI.Xaml.Data;

namespace SipPOS.Converters;
/// <summary>
/// Converts a boolean value indicating button accessibility to a corresponding opacity value.
/// </summary>
public class ButtonAccessibilityToButtonOpacityConverter : IValueConverter
{
    /// <summary>
    /// Converts a boolean value to an opacity value.
    /// </summary>
    /// <param name="value">The boolean value indicating if the button is accessible.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Returns 1 if the button is accessible; otherwise, returns 0.5.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool isAccessible)
        {
            return isAccessible ? 1 : 0.5;
        }
        return 0.5;
    }

    /// <summary>
    /// Converts back an opacity value to a boolean value. This method is not implemented.
    /// </summary>
    /// <param name="value">The opacity value to convert back.</param>
    /// <param name="targetType">The type of the target property. This parameter is not used.</param>
    /// <param name="parameter">An optional parameter to be used in the converter logic. This parameter is not used.</param>
    /// <param name="language">The language of the conversion. This parameter is not used.</param>
    /// <returns>Throws a <see cref="NotImplementedException"/>.</returns>
    /// <exception cref="NotImplementedException">Always thrown since this method is not implemented.</exception>
    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
