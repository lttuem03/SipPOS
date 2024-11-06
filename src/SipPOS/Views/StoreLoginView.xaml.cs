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

/// <summary>
/// Represents the StoreLoginView, is a sub-view of the LoginView.
/// </summary>
public sealed partial class StoreLoginView : Page
{
    /// <summary>
    /// Gets the view model for the store login view.
    /// </summary>
    public StoreLoginViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreLoginView"/> class.
    /// </summary>
    public StoreLoginView()
    {
        this.InitializeComponent();
        ViewModel = new StoreLoginViewModel();
    }

    /// <summary>
    /// Handles the Click event of the store login button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void storeLoginButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStoreLoginButtonClick(storeUsernameTextBox.Text,
                                              storePasswordBox.Password,
                                              errorMessageTextBlock);
    }

    /// <summary>
    /// Handles the Click event of the create new store account button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the Changed event of the store password visible check box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void storePasswordVisibleCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStorePasswordVisibleCheckBoxChanged(storePasswordBox);
    }
}
