namespace SipPOS.Resources.Helper;

static class ExtensionHelper
{
    public static string ToVietnamDongFormatString(this decimal value)
    {
        return value.ToString("C0", new System.Globalization.CultureInfo("vi-VN"));
    }
}