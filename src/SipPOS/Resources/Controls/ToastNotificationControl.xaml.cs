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

namespace SipPOS.Resources.Controls;

/// <summary>
/// A control for displaying toast notifications in the SipPOS application.
/// </summary>
public sealed partial class ToastNotificationControl : UserControl
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ToastNotificationControl"/> class.
    /// </summary>
    public ToastNotificationControl()
    {
        this.InitializeComponent();
    }

    /// <summary>
    /// Shows a toast notification with the specified title, message, and duration.
    /// </summary>
    /// <param name="title">The title of the notification.</param>
    /// <param name="message">The message of the notification.</param>
    /// <param name="duration">The duration in milliseconds for which the notification is displayed. Default is 3000 milliseconds.</param>
    public void Show(string title, string message, int duration = 3000)
    {
        NotificationTitle.Text = title;
        NotificationMessage.Text = message;
        this.Visibility = Visibility.Visible;

        DismissAfterDelay(duration);
    }

    /// <summary>
    /// Dismisses the notification after a specified delay.
    /// </summary>
    /// <param name="delayInMilliseconds">The delay in milliseconds before the notification is dismissed.</param>
    private async void DismissAfterDelay(int delayInMilliseconds)
    {
        await Task.Delay(delayInMilliseconds);
        this.Visibility = Visibility.Collapsed;
    }

    /// <summary>
    /// Handles the click event of the dismiss button to hide the notification.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void DismissButton_Click(object sender, RoutedEventArgs e)
    {
        this.Visibility = Visibility.Collapsed;
    }
}
