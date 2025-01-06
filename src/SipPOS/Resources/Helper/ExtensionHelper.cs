namespace SipPOS.Resources.Helper;

/// <summary>
/// Provides extension methods for various types.
/// </summary>
static class ExtensionHelper
{
    /// <summary>
    /// Converts a decimal value to a formatted string in Vietnamese Dong currency.
    /// </summary>
    /// <param name="value">The decimal value to format.</param>
    /// <returns>A string representing the formatted value in Vietnamese Dong.</returns>
    public static string ToVietnamDongFormatString(this decimal value)
    {
        return value.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
    }
}
