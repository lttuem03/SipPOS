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

namespace SipPOS.Views;

public sealed partial class MainMenuView : Page
{
    public MainMenuView()
    {
        this.InitializeComponent();
    }

    private void toInventoryViewButton_Click(object sender, RoutedEventArgs e)
    {
        if (App.CurrentWindow == null)
            return;

        var rootFrame = App.CurrentWindow.Content as Frame;

        if (rootFrame != null)
        {
            rootFrame.Navigate(typeof(ProductManagementView));
        }
        else
        {
            var errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = "Navigation frame is null.",
                CloseButtonText = "Close"
            };

            _ = errorDialog.ShowAsync();
        }
    }

    private void logoutButton_Click(object sender, RoutedEventArgs e)
    {
        if (App.CurrentWindow == null)
            return;

        var rootFrame = App.CurrentWindow.Content as Frame;

        if (rootFrame != null)
        {
            rootFrame.Navigate(typeof(LoginView));
        }
        else
        {
            var errorDialog = new ContentDialog
            {
                Title = "Error",
                Content = "Navigation frame is null.",
                CloseButtonText = "Close"
            };

            _ = errorDialog.ShowAsync();
        }
    }
}
