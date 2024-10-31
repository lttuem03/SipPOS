using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SipPOS.Controls;

public sealed partial class ToastNotificationControl : UserControl
{
     public ToastNotificationControl()
    {
        this.InitializeComponent();
    }

    public void Show(String title, String message, int duration = 3000)
    {
        NotificationTitle.Text = title;
        NotificationMessage.Text = message;
        this.Visibility = Visibility.Visible;

        DismissAfterDelay(duration);
    }

    private async void DismissAfterDelay(int delayInMilliseconds)
    {
        await Task.Delay(delayInMilliseconds);
        this.Visibility = Visibility.Collapsed;
    }

    private void DismissButton_Click(object sender, RoutedEventArgs e)
    {
        this.Visibility = Visibility.Collapsed;
    }
}
