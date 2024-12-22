using System.ComponentModel;

using Microsoft.UI.Xaml.Controls;

using SipPOS.Views.Setup;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.Account.Interfaces;
using SipPOS.Services.General.Interfaces;
using SipPOS.Models.Entity;

namespace SipPOS.ViewModels.General;

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

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreAccountCreationViewModel"/> class.
    /// </summary>
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

    /// <summary>
    /// Handles the event when the confirm store account creation button is clicked.
    /// </summary>
    /// <param name="rawInfoStoreDto">The raw information of the store entered in the fields.</param>
    /// <param name="repeatPasswordString">The repeated password string.</param>
    public async void HandleConfirmStoreAccountCreationButtonClick(StoreDto rawInfoStoreDto, string repeatPasswordString, ContentDialog comfirmationDialog)
    {
        var allFieldsValid = true;

        if (rawInfoStoreDto.PasswordHash == null)
        {
            return;
        }

        if (rawInfoStoreDto.PasswordHash != repeatPasswordString)
        {
            ConfirmPasswordErrorMessageText = "Xác nhận mật khẩu không khớp.";
            ConfirmPasswordErrorMessageOpacity = 1.0F;
            allFieldsValid = false;
        }

        var storeAccountCreationService = App.GetService<IStoreAccountCreationService>();
        var fieldValidationResults = storeAccountCreationService.ValidateFields(rawInfoStoreDto);
        
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

        if (!allFieldsValid)
        {
            OtherErrorMessageText = "Tạo tài khoản thất bại, có trường không hợp lệ";
            OtherErrorMessageOpacity = 1.0F;
            return;
        }

        OtherErrorMessageOpacity = 0.0F;

        // COMFIRMATION DIALOG
        ContentDialogResult result = await comfirmationDialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
            // ACCOUNT CREATION
            Store? loggedInStore = await storeAccountCreationService.CreateAccountAsync(rawInfoStoreDto);

            if (loggedInStore != null) // SUCCESSFUL
            {
                var storeAuthenticationService = App.GetService<IStoreAuthenticationService>();

                _ = await storeAuthenticationService.LoginAsync(loggedInStore);
                OtherErrorMessageText = "Tạo tài khoản thành công";
                OtherErrorMessageOpacity = 1.0F;

                // Navigate to the main page
                App.NavigateTo(typeof(StoreSetupView));
            }
            else
            {
                OtherErrorMessageText = "Tạo tài khoản thất bại, đã có lỗi xảy ra";
                OtherErrorMessageOpacity = 1.0F;
            }
        }
    }

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Gets or sets the opacity of the store name error message.
    /// </summary>
    public float StoreNameErrorMessageOpacity
    {
        get => _storeNameErrorMessageOpacity;
        set
        {
            _storeNameErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreNameErrorMessageOpacity));
        }
    }
    /// <summary>
    /// Gets or sets the text of the store name error message.
    /// </summary>
    public string StoreNameErrorMessageText
    {
        get => _storeNameErrorMessageText;
        set
        {
            _storeNameErrorMessageText = value;
            OnPropertyChanged(nameof(StoreNameErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store address error message.
    /// </summary>
    public float StoreAddressErrorMessageOpacity
    {
        get => _storeAddressErrorMessageOpacity;
        set
        {
            _storeAddressErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreAddressErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store address error message.
    /// </summary>
    public string StoreAddressErrorMessageText
    {
        get => _storeAddressErrorMessageText;
        set
        {
            _storeAddressErrorMessageText = value;
            OnPropertyChanged(nameof(StoreAddressErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store email error message.
    /// </summary>
    public float StoreEmailErrorMessageOpacity
    {
        get => _storeEmailErrorMessageOpacity;
        set
        {
            _storeEmailErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreEmailErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store email error message.
    /// </summary>
    public string StoreEmailErrorMessageText
    {
        get => _storeEmailErrorMessageText;
        set
        {
            _storeEmailErrorMessageText = value;
            OnPropertyChanged(nameof(StoreEmailErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store telephone error message.
    /// </summary>
    public float StoreTelErrorMessageOpacity
    {
        get => _storeTelErrorMessageOpacity;
        set
        {
            _storeTelErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreTelErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store telephone error message.
    /// </summary>
    public string StoreTelErrorMessageText
    {
        get => _storeTelErrorMessageText;
        set
        {
            _storeTelErrorMessageText = value;
            OnPropertyChanged(nameof(StoreTelErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store username error message.
    /// </summary>
    public float StoreUsernameErrorMessageOpacity
    {
        get => _storeUsernameErrorMessageOpacity;
        set
        {
            _storeUsernameErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StoreUsernameErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store username error message.
    /// </summary>
    public string StoreUsernameErrorMessageText
    {
        get => _storeUsernameErrorMessageText;
        set
        {
            _storeUsernameErrorMessageText = value;
            OnPropertyChanged(nameof(StoreUsernameErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the store password error message.
    /// </summary>
    public float StorePasswordErrorMessageOpacity
    {
        get => _storePasswordErrorMessageOpacity;
        set
        {
            _storePasswordErrorMessageOpacity = value;
            OnPropertyChanged(nameof(StorePasswordErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the store password error message.
    /// </summary>
    public string StorePasswordErrorMessageText
    {
        get => _storePasswordErrorMessageText;
        set
        {
            _storePasswordErrorMessageText = value;
            OnPropertyChanged(nameof(StorePasswordErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the confirm password error message.
    /// </summary>
    public float ConfirmPasswordErrorMessageOpacity
    {
        get => _confirmPasswordErrorMessageOpacity;
        set
        {
            _confirmPasswordErrorMessageOpacity = value;
            OnPropertyChanged(nameof(ConfirmPasswordErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the confirm password error message.
    /// </summary>
    public string ConfirmPasswordErrorMessageText
    {
        get => _confirmPasswordErrorMessageText;
        set
        {
            _confirmPasswordErrorMessageText = value;
            OnPropertyChanged(nameof(ConfirmPasswordErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of other error messages.
    /// </summary>
    public float OtherErrorMessageOpacity
    {
        get => _otherErrorMessageOpacity;
        set
        {
            _otherErrorMessageOpacity = value;
            OnPropertyChanged(nameof(OtherErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of other error messages.
    /// </summary>
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