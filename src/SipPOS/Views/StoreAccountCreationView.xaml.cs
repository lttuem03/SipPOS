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

using SipPOS.DataTransfer.Entity;
using SipPOS.ViewModels;

namespace SipPOS.Views;

/// <summary>
/// Represents the StoreAccountCreationView.
/// </summary>
public sealed partial class StoreAccountCreationView : Page
{
    /// <summary>
    /// Gets the view model for the store account creation view.
    /// </summary>
    public StoreAccountCreationViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreAccountCreationView"/> class.
    /// </summary>
    public StoreAccountCreationView()
    {
        this.InitializeComponent();
        ViewModel = new StoreAccountCreationViewModel();
    }

    /// <summary>
    /// Handles the Click event of the confirm store account creation button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
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

    /// <summary>
    /// Handles the Click event of the cancel store account creation button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void cancelStoreAccountCreationButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleCancelStoreAccountCreationButtonClick();
    }
}
