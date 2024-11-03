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

using SipPOS.ViewModels;

namespace SipPOS.Views;

public sealed partial class LoginView : Page
{
    public LoginViewModel ViewModel { get; }

    public LoginView()
    {
        this.InitializeComponent();
        ViewModel = new LoginViewModel();
        ViewModel.SelectViewUponLoad(loginNavigationView, loginNavigationFrame);
    }

    private void loginNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        ViewModel.HandleLoginNavigationViewSelectionChanged(loginNavigationFrame, args);
    }
}