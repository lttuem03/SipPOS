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

/// <summary>
/// Represents the LoginView.
/// </summary>
public sealed partial class LoginView : Page
{
    /// <summary>
    /// Gets the view model for the login view.
    /// </summary>
    public LoginViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginView"/> class.
    /// </summary>
    public LoginView()
    {
        this.InitializeComponent();
        ViewModel = new LoginViewModel();
        ViewModel.SelectViewUponLoad(loginNavigationView, loginNavigationFrame);
    }

    /// <summary>
    /// Handles the selection changed event of the login navigation view.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The event data.</param>
    private void loginNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        ViewModel.HandleLoginNavigationViewSelectionChanged(loginNavigationFrame, args);
    }
}
