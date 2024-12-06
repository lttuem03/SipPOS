using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

using SipPOS.ViewModels.Login;

namespace SipPOS.Views.Login;

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
        ViewModel = new LoginViewModel(loginNavigationView, storeLoginNavigationViewItem, loginNavigationFrame);
        LoginViewModel.UpdateView();
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

    private void exitProgramButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
    {
        App.Current.Exit();
    }
}
