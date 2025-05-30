﻿using System.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.General;
using SipPOS.Views.Login;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.Authentication.Interfaces;
using SipPOS.Services.Authentication.Implementations;
using SipPOS.Services.Configuration.Interfaces;

namespace SipPOS.ViewModels.Login;

/// <summary>
/// ViewModel for handling store login and logout operations.
/// </summary>
public class StoreLoginViewModel : INotifyPropertyChanged
{
    // Data-bound properties
    private Visibility _storeLoginFormVisibility;
    private Visibility _storeLogoutButtonVisibility;
    private string _storeLastLoginStatus;

    private bool _saveStoreCredentials;

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreLoginViewModel"/> class.
    /// </summary>
    public StoreLoginViewModel()
    {
        _saveStoreCredentials = false;
        _storeLastLoginStatus = string.Empty;

        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            StoreLoginFormVisibility = Visibility.Visible;
            StoreLogoutButtonVisibility = Visibility.Collapsed;
            return;
        }

        StoreLoginFormVisibility = (storeAuthenticationService.Context.LoggedIn == true) ? Visibility.Collapsed : Visibility.Visible;
        StoreLogoutButtonVisibility = (storeAuthenticationService.Context.LoggedIn == true) ? Visibility.Visible : Visibility.Collapsed;

        if (storeAuthenticationService.Context.LoggedIn == true &&
            storeAuthenticationService.Context.CurrentStore != null)
        {
            _storeLastLoginStatus = $"Đăng nhập lần cuối: {storeAuthenticationService.Context.CurrentStore.LastLogin.ToString()}";
        }
    }

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
            return;
        }

        // Load up the configuration for the store
        var currentStoreId = storeAuthenticationService.GetCurrentStoreId();
        var storeConfigurationService = App.GetService<IConfigurationService>();

        await storeConfigurationService.LoadAsync(currentStoreId);
        
        // Save credentials if "Save credentials" was checked or clear credentials if it was unchecked
        var storeCredentialsService = App.GetService<IStoreCredentialsService>();

        if (_saveStoreCredentials)
        {
            storeCredentialsService.SaveCredentials(storeUsername, storePassword);
        }
        else
        {
            storeCredentialsService.ClearCredentials();
        }

        // Change the contents of StoreLoginView
        if (storeAuthenticationService is StoreAuthenticationService service)
        {
            StoreLoginFormVisibility = (service.Context.LoggedIn == true) ? Visibility.Collapsed : Visibility.Visible;
            StoreLogoutButtonVisibility = (service.Context.LoggedIn == true) ? Visibility.Visible : Visibility.Collapsed;
        }

        LoginViewModel.UpdateView();
        App.NavigateTo(typeof(LoginView));
        // TODO: UNDO THIS WHEN DONE DEVELOPING THE STORE SETUP PAGES
        //App.NavigateTo(typeof(StoreSetupView));
    }

    public async void HandleStoreLogoutButtonClick(XamlRoot xamlRoot)
    {
        ContentDialog confirmLogoutContentDialog = new ContentDialog
        {
            XamlRoot = xamlRoot,
            Title = "Xác nhận đăng xuất",
            Content = "Bạn chắc chắn muôn đăng xuất tài khoản cửa hàng chứ?",
            PrimaryButtonText = "Xác nhận",
            CloseButtonText = "Hủy"
        };

        var result = await confirmLogoutContentDialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
        {
            return;
        }

        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService ||
            App.GetService<IStaffAuthenticationService>() is not StaffAuthenticationService staffAuthenticationService)
        {
            return;
        }

        staffAuthenticationService.Logout();
        storeAuthenticationService.Logout();

        var storeCredentialsService = App.GetService<IStoreCredentialsService>();

        storeCredentialsService.ClearCredentials();

        // Clear current shift if needed

        // Change the contents of StoreLoginView
        StoreLoginFormVisibility = (storeAuthenticationService.Context.LoggedIn == true) ? Visibility.Collapsed : Visibility.Visible;
        StoreLogoutButtonVisibility = (storeAuthenticationService.Context.LoggedIn == true) ? Visibility.Visible : Visibility.Collapsed;

        LoginViewModel.UpdateView();
        App.NavigateTo(typeof(LoginView));
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
    /// Handles the event when the save store credentials checkbox is checked.
    /// </summary>
    public void HandleSaveStoreCredentialsCheckBoxChecked()
    {
        _saveStoreCredentials = true;
    }

    /// <summary>
    /// Handles the event when the save store credentials checkbox is unchecked.
    /// </summary>
    public void HandleSaveStoreCredentialsCheckBoxUnchecked()
    {
        _saveStoreCredentials = false;
    }

    /// <summary>
    /// Handles the event when the create new store account button is clicked.
    /// </summary>
    public void HandleCreateNewStoreAccountButtonClick()
    {
        App.NavigateTo(typeof(StoreAccountCreationView));
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Gets or sets the visibility of the store login form.
    /// </summary>
    public Visibility StoreLoginFormVisibility
    {
        get => _storeLoginFormVisibility;
        set
        {
            _storeLoginFormVisibility = value;
            OnPropertyChanged(nameof(StoreLoginFormVisibility));
        }
    }

    /// <summary>
    /// Gets or sets the visibility of the store logout button.
    /// </summary>
    public Visibility StoreLogoutButtonVisibility
    {
        get => _storeLogoutButtonVisibility;
        set
        {
            _storeLogoutButtonVisibility = value;
            OnPropertyChanged(nameof(StoreLogoutButtonVisibility));
        }
    }

    /// <summary>
    /// Gets or sets the last login status of the store.
    /// </summary>
    public string StoreLastLoginStatus
    {
        get => _storeLastLoginStatus;
        set
        {
            _storeLastLoginStatus = value;
            OnPropertyChanged(nameof(StoreLastLoginStatus));
        }
    }
}
