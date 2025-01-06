using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.Login;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;

namespace SipPOS.ViewModels.Login;

/// <summary>  
/// ViewModel for handling navigation to other sub-LoginView in the LoginView.  
/// </summary>  
public class LoginViewModel
{
    public static bool Instantiated {  get; private set; }

    // UI Elements of the LoginView itself, we need it here
    // to update the view based on the authentication state
    private static NavigationView? _navigationView;
    private static NavigationViewItem? _storeLoginNavigationViewItem;
    private static Frame? _loginNavigationFrame;

    private LoginViewModel()
    {
        // Default constructor is hidden
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginViewModel"/> class.
    /// </summary>
    /// <param name="navigationView">The navigation view to select the item in.</param>
    /// <param name="storeLoginNavigationViewItem">The navigation view item for store login.</param>
    /// <param name="loginNavigationFrame">The frame to navigate within.</param>
    public LoginViewModel(NavigationView navigationView,
                          NavigationViewItem storeLoginNavigationViewItem,
                          Frame loginNavigationFrame)
    {
        Instantiated = true;
        _navigationView = navigationView;
        _storeLoginNavigationViewItem = storeLoginNavigationViewItem;
        _loginNavigationFrame = loginNavigationFrame;
    }

    /// <summary>  
    /// Handles the selection change in the login navigation view.  
    /// </summary>  
    /// <param name="loginNavigationFrame">The frame to navigate within.</param>  
    /// <param name="args">The event arguments containing the selected item.</param>  
    public void HandleLoginNavigationViewSelectionChanged(Frame loginNavigationFrame, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer != null)
        {
            var selectedItemTag = args.SelectedItemContainer.Tag.ToString();

            switch (selectedItemTag)
            {
                case "StaffLogin":
                    loginNavigationFrame.Navigate(typeof(StaffLoginView));
                    break;
                case "StoreLogin":
                    loginNavigationFrame.Navigate(typeof(StoreLoginView));
                    break;
            }
        }
    }

    /// <summary>  
    /// Selects the initial view upon loading based on the authentication state.  
    /// </summary>  
    /// <param name="navigationView">The navigation view to select the item in.</param>  
    /// <param name="loginNavigationFrame">The frame to navigate within.</param>  
    public static void UpdateView()
    {
        if (!Instantiated)
        {
            return;
        }

        if (_navigationView == null ||
            _storeLoginNavigationViewItem == null ||
            _loginNavigationFrame == null)
        {
            return;
        }

        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            return;
        }

        var initialViewTag = "";

        if (storeAuthenticationService.Context.LoggedIn)
        {
            initialViewTag = "StaffLogin";

            if (storeAuthenticationService.Context.CurrentStore != null)
            {
               _storeLoginNavigationViewItem.Content = $"Đã đăng nhập ({storeAuthenticationService.Context.CurrentStore.Username})";
            }
        }
        else
        {
            initialViewTag = "StoreLogin";
            _storeLoginNavigationViewItem.Content = "Đăng nhập cửa hàng";
        }

        var initialItem = _navigationView.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => item.Tag.ToString() == initialViewTag);

        if (initialItem != null)
        {
            _navigationView.SelectedItem = initialItem;
            switch (initialViewTag)
            {
                case "StaffLogin":
                    _loginNavigationFrame.Navigate(typeof(StaffLoginView));
                    break;
                case "StoreLogin":
                    _loginNavigationFrame.Navigate(typeof(StoreLoginView));
                    break;
            }
        }
    }
}
