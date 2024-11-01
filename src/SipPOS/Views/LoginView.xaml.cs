using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;


namespace SipPOS.Views;


public sealed partial class LoginView : Page
{
    private TextBox? _selectedTextBox;
    private PasswordBox? _selectedPasswordBox;

    public LoginView()
    {
        this.InitializeComponent();
    }

    private void TextBox_GotFocus(object sender, RoutedEventArgs e)
    {
        _selectedTextBox = sender as TextBox;
        _selectedPasswordBox = null;
    }

    private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
    {
        _selectedPasswordBox = sender as PasswordBox;
        _selectedTextBox = null;
    }

    private void NumpadButton_Click(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;

        if (button == null)
        {
            return;
        }

        var number = button.Content.ToString();

        if (_selectedTextBox != null)
        {
            _selectedTextBox.Text += number;
        }
        else if (_selectedPasswordBox != null)
        {
            _selectedPasswordBox.Password += number;
        }
    }

    private void selectPrefixComboBox_GotFocus(object sender, RoutedEventArgs e)
    {
        _selectedTextBox = null;
        _selectedPasswordBox = null;
    }
}
