using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Login;

namespace SipPOS.Views.Login;

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
        ViewModel.HandleCreateNewStoreAccountButtonClick();
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

    /// <summary>
    /// Handles the Checked event of the save store credentials check box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void saveStoreCredentialsCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleSaveStoreCredentialsCheckBoxChecked();
    }

    /// <summary>
    /// Handles the Unchecked event of the save store credentials check box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void saveStoreCredentialsCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleSaveStoreCredentialsCheckBoxUnchecked();
    }

    /// <summary>
    /// Handles the Click event of the store logout button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void storeLogoutButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStoreLogoutButtonClick(this.XamlRoot);
    }
}
