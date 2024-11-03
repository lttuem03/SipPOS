using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Controls;

using SipPOS.Services.Implementations;
using SipPOS.Services.Interfaces;
using SipPOS.Views;

namespace SipPOS.ViewModels;

public class StoreLoginViewModel
{
    public async void HandleStoreLoginButtonClick(string storeUsername, string storePassword, TextBlock errorMessageTextBlock)
    {
        var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();

        var loginSuccessful = await storeAuthenticationService.LoginAsync(storeUsername, storePassword);

        if (!loginSuccessful)
        {
            errorMessageTextBlock.Opacity = 1.0;
        }
        else
        {
            // Navigate to the main menu view
            if (App.CurrentWindow == null)
                return;

            var rootFrame = App.CurrentWindow.Content as Frame;

            if (rootFrame != null)
            {
                rootFrame.Navigate(typeof(MainMenuView));
            }
            else
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = "Navigation frame is null.",
                    CloseButtonText = "Close"
                };

                _ = errorDialog.ShowAsync();
            }
        }
    }
}
