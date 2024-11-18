using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Controls;

using SipPOS.Services.General.Interfaces;
using SipPOS.Views;

namespace SipPOS.ViewModels;

public class StoreLoginViewModel
{
    /// <summary>
    /// Handles the event when the store login button is clicked.
    /// </summary>
    /// <param name="storeUsername">The username of the store.</param>
    /// <param name="storePassword">The password of the store.</param>
    /// <param name="errorMessageTextBlock">The text block to display error messages.</param>
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
            App.NavigateTo(typeof(MainMenuView));
        }
    }

    /// <summary>
    /// Handles the event when the store password visibility checkbox is changed.
    /// </summary>
    /// <param name="storePasswordBox">The password box of the store.</param>
    public void HandleStorePasswordVisibleCheckBoxChanged(PasswordBox storePasswordBox)
    {
        storePasswordBox.PasswordRevealMode = (storePasswordBox.PasswordRevealMode == PasswordRevealMode.Hidden) ?
                                              PasswordRevealMode.Visible : PasswordRevealMode.Hidden;
    }

    /// <summary>
    /// Handles the event when the create new store account button is clicked.
    /// </summary>
    public void HandleCreateNewStoreAccountButtonClick()
    {
        App.NavigateTo(typeof(StoreAccountCreationView));
    }
}
