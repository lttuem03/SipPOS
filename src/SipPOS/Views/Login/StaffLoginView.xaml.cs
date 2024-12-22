using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.ViewModels.Login;

namespace SipPOS.Views.Login;

/// <summary>
/// Represents the StaffLoginView, is a sub-view of the LoginView.
/// </summary>
public sealed partial class StaffLoginView : Page
{
    /// <summary>
    /// Gets the view model for the staff login view.
    /// </summary>
    public StaffLoginViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffLoginView"/> class.
    /// </summary>
    public StaffLoginView()
    {
        this.InitializeComponent();
        ViewModel = new StaffLoginViewModel();
    }

    /// <summary>
    /// Handles the GotFocus event of the staff ID text box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void staffIdTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStaffIdTextBoxGotFocus();
    }

    /// <summary>
    /// Handles the GotFocus event of the select prefix combo box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void selectPrefixComboBox_GotFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleSelectPrefixComboBoxGotFocus();
    }

    /// <summary>
    /// Handles the Click event of the numpad button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void numpadButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;

        if (button == null)
        {
            return;
        }

        var number = button.Content.ToString();

        if (number == null)
        {
            return;
        }

        ViewModel.HandleNumpadButtonClick(number);
    }

    private void numpadBackspaceButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;

        if (button == null)
        {
            return;
        }

        ViewModel.HandleNumpadBackspaceButtonClick();
    }

    /// <summary>
    /// Handles the Click event of the staff login button.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void staffLoginButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStaffLoginButtonClick(errorMessageTextBlock);
    }
}
