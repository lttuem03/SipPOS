using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.General;
using SipPOS.DataTransfer.Entity;
using SipPOS.Views.Login;

namespace SipPOS.Views.General;

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

        ViewModel.HandleConfirmStoreAccountCreationButtonClick(rawInfoStoreDto, repeatPasswordString, confirmStoreInformationContentDialog);
    }

    private void confirmStoreInformationContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
    {
        // Load the information into the content dialog

        // Doing it this way is way more straight-forward than create Dto
        // and pass it to a ViewModel's method

        confirmStoreNameTextBlock.Text = storeNameFieldTextBox.Text;
        confirmStoreAddressTextBlock.Text = storeAddressFieldTextBox.Text;
        confirmStoreEmailTextBlock.Text = storeEmailFieldTextBox.Text;
        confirmStoreTelTextBlock.Text = storeTelFieldTextBox.Text;
        confirmStoreUsernameTextBlock.Text = storeUsernameFieldTextBox.Text;
    }

    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(LoginView));
    }
}
