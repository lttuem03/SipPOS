using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SipPOS.Views.General;

namespace SipPOS.Views.Cashier;

public sealed partial class CashierMenuView : Page
{
    public CashierMenuView()
    {
        this.InitializeComponent();
    }

    private void goBackButton_Click(object sender, RoutedEventArgs e)
    {
        App.NavigateTo(typeof(MainMenuView));
    }
}
