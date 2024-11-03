using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.UI.Xaml.Controls;

using SipPOS.Models;
using SipPOS.DataTransfer;
using SipPOS.Services.Interfaces;
using SipPOS.Services.Implementations;
using SipPOS.Views;

namespace SipPOS.ViewModels;

public class StoreAccountCreationViewModel : INotifyPropertyChanged
{
    // Properties used for data binding -> Implement setter that fires a PropertyChanged event
    private float _storeNameErrorMessageOpacity;
    private string _storeNameErrorMessageText;

    private float _storeAddressErrorMessageOpacity;
    private string _storeAddressErrorMessageText;

    private float _storeEmailErrorMessageOpacity;
    private string _storeEmailErrorMessageText;

    private float _storeTelErrorMessageOpacity;
    private string _storeTelErrorMessageText;

    private float _storeUsernameErrorMessageOpacity;
    private string _storeUsernameErrorMessageText;

    private float _storePasswordErrorMessageOpacity;
    private string _storePasswordErrorMessageText;

    private float _confirmPasswordErrorMessageOpacity;
    private string _confirmPasswordErrorMessageText;

    private float _otherErrorMessageOpacity;
    private string _otherErrorMessageMessageText;

    public StoreAccountCreationViewModel()
    {
        _storeNameErrorMessageOpacity = 0.0F;
        _storeNameErrorMessageText = String.Empty;

        _storeAddressErrorMessageOpacity = 0.0F;
        _storeAddressErrorMessageText = String.Empty;

        _storeEmailErrorMessageOpacity = 0.0F;
        _storeEmailErrorMessageText = String.Empty;

        _storeTelErrorMessageOpacity = 0.0F;
        _storeTelErrorMessageText = String.Empty;

        _storeUsernameErrorMessageOpacity = 0.0F;
        _storeUsernameErrorMessageText = String.Empty;

        _storePasswordErrorMessageOpacity = 0.0F;
        _storePasswordErrorMessageText = String.Empty;

        _confirmPasswordErrorMessageOpacity = 0.0F;
        _confirmPasswordErrorMessageText = String.Empty;

        _otherErrorMessageOpacity = 0.0F;
        _otherErrorMessageMessageText = String.Empty;
    }

    public async void HandleConfirmStoreAccountCreationButtonClick(StoreDto rawInfoStoreDto, string repeatPasswordString)
    {
        if (rawInfoStoreDto.PasswordHash == null)
        {
            return;
        }

        if (rawInfoStoreDto.PasswordHash != repeatPasswordString)
        {
            ConfirmPasswordErrorMessageText = "Xác nhận mật khẩu không khớp.";
            ConfirmPasswordErrorMessageOpacity = 1.0F;
        }

        var storeAccountCreationService = App.GetService<IStoreAccountCreationService>();

        var fieldValidationResults = storeAccountCreationService.ValidateFields(rawInfoStoreDto);
        var allFieldsValid = true;

        foreach (var fieldValidationResult in fieldValidationResults)
        {
            var field = fieldValidationResult.Key;
            var validationResult = fieldValidationResult.Value;
            
            var opacity = 0.0F;

            if (validationResult != "OK")
            {
                opacity = 1.0F;
                allFieldsValid = false;
            }
            
            switch (field)
            {
                case "name":
                    StoreNameErrorMessageText = validationResult;
                    StoreNameErrorMessageOpacity = opacity;
                    break;
                case "address":
                    StoreAddressErrorMessageText = validationResult;
                    StoreAddressErrorMessageOpacity = opacity;
                    break;
                case "email":
                    StoreEmailErrorMessageText = validationResult;
                    StoreEmailErrorMessageOpacity = opacity;
                    break;
                case "tel":
                    StoreTelErrorMessageText = validationResult;
                    StoreTelErrorMessageOpacity = opacity;
                    break;
                case "username":
                    StoreUsernameErrorMessageText = validationResult;
                    StoreUsernameErrorMessageOpacity = opacity;
                    break;
                case "password":
                    StorePasswordErrorMessageText = validationResult;
                    StorePasswordErrorMessageOpacity = opacity;
                    break;
            }
        }

        // ACCOUNT CREATION
        if (allFieldsValid)
        {
            Store? loggedInStore = await storeAccountCreationService.CreateAccountAsync(rawInfoStoreDto);

            if (loggedInStore != null) // SUCCESSFUL
            {
                var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();

                _ = await storeAuthenticationService.LoginAsync(loggedInStore);
                OtherErrorMessageText = "Tạo tài khoản thành công";
                OtherErrorMessageOpacity = 1.0F;

                // Navigate to the main page
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
            else
            {
                OtherErrorMessageText = "Tạo tài khoản thất bại, đã có lỗi xảy ra";
                OtherErrorMessageOpacity = 1.0F;
            }
        }
        else
        {
            OtherErrorMessageText = "Tạo tài khoản thất bại, có trường không hợp lệ";
            OtherErrorMessageOpacity = 1.0F;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // Store Name error-handling properties
    public float StoreNameErrorMessageOpacity
    {
        get => _storeNameErrorMessageOpacity;
        set
        {
            _storeNameErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreNameErrorMessageOpacity));
        }
    }
    public string StoreNameErrorMessageText
    {
        get => _storeNameErrorMessageText;
        set
        {
            _storeNameErrorMessageText = value;
            OnPropertyChanged(nameof(StoreNameErrorMessageText));
        }
    }

    // Store Address error-handling properties
    public float StoreAddressErrorMessageOpacity
    {
        get => _storeAddressErrorMessageOpacity;
        set
        {
            _storeAddressErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreAddressErrorMessageOpacity));
        }
    }

    public string StoreAddressErrorMessageText
    {
        get => _storeAddressErrorMessageText;
        set
        {
            _storeAddressErrorMessageText = value;
            OnPropertyChanged(nameof(StoreAddressErrorMessageText));
        }
    }

    // Store Email error-handling properties
    public float StoreEmailErrorMessageOpacity
    {
        get => _storeEmailErrorMessageOpacity;
        set
        {
            _storeEmailErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreEmailErrorMessageOpacity));
        }
    }

    public string StoreEmailErrorMessageText
    {
        get => _storeEmailErrorMessageText;
        set
        {
            _storeEmailErrorMessageText = value;
            OnPropertyChanged(nameof(StoreEmailErrorMessageText));
        }
    }

    // Store Tel error-handling properties
    public float StoreTelErrorMessageOpacity
    {
        get => _storeTelErrorMessageOpacity;
        set
        {
            _storeTelErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreTelErrorMessageOpacity));
        }
    }

    public string StoreTelErrorMessageText
    {
        get => _storeTelErrorMessageText;
        set
        {
            _storeTelErrorMessageText = value;
            OnPropertyChanged(nameof(StoreTelErrorMessageText));
        }
    }

    // Store Username error-handling properties
    public float StoreUsernameErrorMessageOpacity
    {
        get => _storeUsernameErrorMessageOpacity;
        set
        {
            _storeUsernameErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreUsernameErrorMessageOpacity));
        }
    }

    public string StoreUsernameErrorMessageText
    {
        get => _storeUsernameErrorMessageText;
        set
        {
            _storeUsernameErrorMessageText = value;
            OnPropertyChanged(nameof(StoreUsernameErrorMessageText));
        }
    }

    // Store Password error-handling properties
    public float StorePasswordErrorMessageOpacity
    {
        get => _storePasswordErrorMessageOpacity;
        set
        {
            _storePasswordErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StorePasswordErrorMessageOpacity));
        }
    }

    public string StorePasswordErrorMessageText
    {
        get => _storePasswordErrorMessageText;
        set
        {
            _storePasswordErrorMessageText = value;
            OnPropertyChanged(nameof(StorePasswordErrorMessageText));
        }
    }

    // Store ConfirmPassword error-handling properties
    public float ConfirmPasswordErrorMessageOpacity
    {
        get => _confirmPasswordErrorMessageOpacity;
        set
        {
            _confirmPasswordErrorMessageOpacity = value;
            OnPropertyChanged(nameof(ConfirmPasswordErrorMessageOpacity));
        }
    }

    public string ConfirmPasswordErrorMessageText
    {
        get => _confirmPasswordErrorMessageText;
        set
        {
            _confirmPasswordErrorMessageText = value;
            OnPropertyChanged(nameof(ConfirmPasswordErrorMessageText));
        }
    }

    // Other error-handling properties
    public float OtherErrorMessageOpacity
    {
        get => _otherErrorMessageOpacity;
        set
        {
            _otherErrorMessageOpacity = value;
            OnPropertyChanged(nameof(OtherErrorMessageOpacity));
        }
    }

    public string OtherErrorMessageText
    {
        get => _otherErrorMessageMessageText;
        set
        {
            _otherErrorMessageMessageText = value;
            OnPropertyChanged(nameof(OtherErrorMessageText));
        }
    }
}