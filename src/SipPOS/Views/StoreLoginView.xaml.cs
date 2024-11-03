using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

using SipPOS.ViewModels;

namespace SipPOS.Views;

public sealed partial class StoreLoginView : Page
{
    public StoreLoginViewModel ViewModel { get; }

    public StoreLoginView()
    {
        this.InitializeComponent();
        ViewModel = new StoreLoginViewModel();
    }

    private void storeLoginButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStoreLoginButtonClick(storeUsernameTextBox.Text, 
                                              storePasswordBox.Password,
                                              errorMessageTextBlock);
    }

    private void createNewStoreAccountButton_Click(object sender, RoutedEventArgs e)
    {
        if (App.CurrentWindow == null)
            return;

        var rootFrame = App.CurrentWindow.Content as Frame;

        if (rootFrame != null)
        {
            rootFrame.Navigate(typeof(StoreAccountCreationView));
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
