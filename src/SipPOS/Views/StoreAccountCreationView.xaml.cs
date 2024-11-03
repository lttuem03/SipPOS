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

using SipPOS.DataTransfer;
using SipPOS.ViewModels;

namespace SipPOS.Views;

public sealed partial class StoreAccountCreationView : Page
{
    public StoreAccountCreationViewModel ViewModel { get; }

    public StoreAccountCreationView()
    {
        this.InitializeComponent();
        ViewModel = new StoreAccountCreationViewModel();
    }

    private void confirmStoreAccountCreationButton_Click(object sender, RoutedEventArgs e)
    {
        StoreDto rawInfoStoreDto = new StoreDto
        {
            Name = storeNameFieldTextBox.Text,
            Address = storeAddressFieldTextBox.Text,
            Email = storeEmailFieldTextBox.Text,
            Tel = storeTelFieldTextBox.Text,
            Username = storeUsernameFieldTextBox.Text,
            PasswordHash = storePasswordFieldPasswordBox.Password  // UN-HASHED AT THIS POINT
        };

        var repeatPasswordString = confirmStorePasswordFieldPasswordBox.Password;

        ViewModel.HandleConfirmStoreAccountCreationButtonClick(rawInfoStoreDto, repeatPasswordString);
    }

    private void cancelStoreAccountCreationButton_Click(object sender, RoutedEventArgs e)
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
