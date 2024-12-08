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
    public void SelectViewUponLoad(NavigationView navigationView, 
                                   NavigationViewItem storeLoginNavigationViewItem, 
                                   Frame loginNavigationFrame)
    {
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
                storeLoginNavigationViewItem.Content = $"Đã đăng nhập ({storeAuthenticationService.Context.CurrentStore.Username})";
            }
        }
        else
        {
            initialViewTag = "StoreLogin";
            storeLoginNavigationViewItem.Content = "Đăng nhập cửa hàng";
        }

        var initialItem = navigationView.MenuItems
            .OfType<NavigationViewItem>()
            .FirstOrDefault(item => item.Tag.ToString() == initialViewTag);

        if (initialItem != null)
        {
            navigationView.SelectedItem = initialItem;
            switch (initialViewTag)
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
}
