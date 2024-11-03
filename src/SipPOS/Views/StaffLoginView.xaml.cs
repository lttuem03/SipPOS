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

public sealed partial class StaffLoginView : Page
{
    public StaffLoginViewModel ViewModel { get; }

    public StaffLoginView()
    {
        this.InitializeComponent();
        ViewModel = new StaffLoginViewModel();
    }

    private void staffIdTextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStaffIdTextBoxGotFocus();
    }

    private void staffPasswordBox_GotFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStaffPasswordBoxGotFocus();
    }

    private void selectPrefixComboBox_GotFocus(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleSelectPrefixComboBoxGotFocus();
    }

    private void staffPasswordVisibleCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleStaffPasswordVisibleCheckBoxChanged(staffPasswordBox);
    }

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
