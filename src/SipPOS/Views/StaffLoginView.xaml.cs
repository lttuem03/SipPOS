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
    /// Handles the GotFocus event of the staff password box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void staffPasswordBox_GotFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStaffPasswordBoxGotFocus();
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
    /// Handles the Changed event of the staff password visible check box.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void staffPasswordVisibleCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStaffPasswordVisibleCheckBoxChanged(staffPasswordBox);
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
}
