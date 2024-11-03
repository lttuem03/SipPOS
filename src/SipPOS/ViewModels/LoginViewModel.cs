using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;

using SipPOS.Services.Interfaces;
using SipPOS.Services.Implementations;
using SipPOS.Views;

namespace SipPOS.ViewModels;

public class LoginViewModel
{
    public LoginViewModel()
    {

    }

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

    public void SelectViewUponLoad(NavigationView navigationView, Frame loginNavigationFrame)
    {
        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            return;
        }

        var initialViewTag = "";

        if (storeAuthenticationService.Context.LoggedIn)
        {
            initialViewTag = "StaffLogin";
        }
        else
        {
            initialViewTag = "StoreLogin";
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
