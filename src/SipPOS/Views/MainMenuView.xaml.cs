using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SipPOS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SipPOS.Views;

/// <summary>
/// Represents the MainMenuView.
/// </summary>
public sealed partial class MainMenuView : Page
{
    /// <summary>
    /// Gets the view model for the main menu view.
    /// </summary>
    public MainMenuViewModel ViewModel { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MainMenuView"/> class.
    /// </summary>
    public MainMenuView()
    {
        this.InitializeComponent();
        ViewModel = new MainMenuViewModel();
    }

    /// <summary>
    /// Handles the click event of the button to navigate to the inventory view.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void toProductManagementViewButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleToProductManagementViewButtonClick();
    }

    /// <summary>
    /// Handles the click event of the button to change the ID.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void changeIdButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.HandleChangeIdButtonClick();
    }
}
