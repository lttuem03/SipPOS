using System.ComponentModel;
using System.Text.RegularExpressions;

using Microsoft.UI.Xaml.Controls;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Resources.Controls;
using WinRT.SipPOSVtableClasses;
using SipPOS.Models.Entity;

namespace SipPOS.ViewModels.Configuration;

/// <summary>
/// ViewModel for store configuration.
/// </summary>
public class StoreConfigurationViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    // Contextual properties
    private readonly Store? _currentStore;

    // Data-bound properties
    private string _editStoreNameText = string.Empty;
    private string _editStoreAddressText = string.Empty;
    private string _editStoreEmailText = string.Empty;
    private string _editStoreTelText = string.Empty;
    private string _operatingHoursText = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreConfigurationViewModel"/> class.
    /// </summary>
    public StoreConfigurationViewModel()
    {
        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
        {
            return;
        }

        if (storeAuthenticationService.Context.CurrentStore == null)
        {
            return;
        }

        _currentStore = storeAuthenticationService.Context.CurrentStore;

        EditStoreNameText = _currentStore.Name;
        EditStoreAddressText = _currentStore.Address;
        EditStoreEmailText = _currentStore.Email;
        EditStoreTelText = _currentStore.Tel;
    }

    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void HandleEditableTextFieldTextModified(Button saveChangesOnStoreConfigurationButton)
    {
        if (_currentStore == null)
        {
            return;
        }

        if (EditStoreNameText != _currentStore.Name ||
            EditStoreAddressText != _currentStore.Address ||
            EditStoreEmailText != _currentStore.Email ||
            EditStoreTelText != _currentStore.Tel)
        {
            saveChangesOnStoreConfigurationButton.IsEnabled = true;
        }
    }

    public void HandleSaveChangesOnStoreConfigurationButtonClick()
    {
        
    }

    public void HandleCancelChangesOnStoreConfigurationButtonClick(Button saveChangesOnStoreConfigurationButton)
    {
        resetToCurrentStoreConfiguration();

        saveChangesOnStoreConfigurationButton.IsEnabled = false;
    }

    public void HandleStoreNameEditableTextFieldSaveClicked
    (
        TeachingTip storeNameErrorMessageTeachingTip,
        EditableTextField storeNameEditableTextField)
    {
        if (string.IsNullOrEmpty(EditStoreNameText))
        {
            storeNameErrorMessageTeachingTip.Subtitle = "Tên cửa hàng không được rỗng";
            storeNameErrorMessageTeachingTip.IsOpen = true;
            storeNameEditableTextField.ResetState();
            EditStoreNameText = _currentStore == null ? "Lỗi đăng nhập" : _currentStore.Name;
        }
    }

    public void HandleStoreAddressEditableTextFieldSaveClicked
    (
        TeachingTip storeAddressErrorMessageTeachingTip,
        EditableTextField storeAddressEditableTextField)
    {
        if (string.IsNullOrEmpty(EditStoreAddressText))
        {
            storeAddressErrorMessageTeachingTip.Subtitle = "Địa chỉ cửa hàng không được rỗng";
            storeAddressErrorMessageTeachingTip.IsOpen = true;
            storeAddressEditableTextField.ResetState();
            EditStoreAddressText = _currentStore == null ? "Lỗi đăng nhập" : _currentStore.Address;
        }
    }

    public void HandleStoreEmailEditableTextFieldSaveClicked
    (
        TeachingTip storeEmailErrorMessageTeachingTip,
        EditableTextField storeEmailEditableTextField)
    {
        if (string.IsNullOrEmpty(EditStoreEmailText))
        {
            storeEmailErrorMessageTeachingTip.Subtitle = "Email cửa hàng không được rỗng";
            storeEmailErrorMessageTeachingTip.IsOpen = true;
            storeEmailEditableTextField.ResetState();
            EditStoreEmailText = _currentStore == null ? "Lỗi đăng nhập" : _currentStore.Email;
            return;
        }

        var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        if (!Regex.IsMatch(EditStoreEmailText, emailPattern))
        {
            storeEmailErrorMessageTeachingTip.Subtitle = "Email sai định dạng";
            storeEmailErrorMessageTeachingTip.IsOpen = true;
            storeEmailEditableTextField.ResetState();
            EditStoreEmailText = _currentStore == null ? "Lỗi đăng nhập" : _currentStore.Email;
        }
    }

    public void HandleStoreTelEditableTextFieldSaveClicked
    (
        TeachingTip storeTelErrorMessageTeachingTip,
        EditableTextField storeTelEditableTextField)
    {
        if (string.IsNullOrEmpty(EditStoreTelText))
        {
            storeTelErrorMessageTeachingTip.Subtitle = "SĐT cửa hàng không được rỗng";
            storeTelErrorMessageTeachingTip.IsOpen = true;
            storeTelEditableTextField.ResetState();
            EditStoreTelText = _currentStore == null ? "Lỗi đăng nhập" : _currentStore.Tel;

            return;
        }

        var telPattern = @"^(0|\+84)\d{9,12}$";

        if (!Regex.IsMatch(EditStoreTelText, telPattern))
        {
            storeTelErrorMessageTeachingTip.Subtitle = "SĐT sai định dạng";
            storeTelErrorMessageTeachingTip.IsOpen = true;
            storeTelEditableTextField.ResetState();
            EditStoreTelText = _currentStore == null ? "Lỗi đăng nhập" : _currentStore.Tel;
        }
    }

    private void resetToCurrentStoreConfiguration()
    {
        EditStoreNameText = _currentStore == null ? "Lỗi đăng nhập" : _currentStore.Name;
        EditStoreAddressText = _currentStore == null ? "Lỗi đăng nhập" : _currentStore.Address;
        EditStoreEmailText = _currentStore == null ? "Lỗi đăng nhập" : _currentStore.Email;
        EditStoreTelText = _currentStore == null ? "Lỗi đăng nhập" : _currentStore.Tel;
    }

    /// <summary>
    /// Gets or sets the store name text.
    /// </summary>
    public string EditStoreNameText
    {
        get => _editStoreNameText;
        set
        {
            _editStoreNameText = value;
            OnPropertyChanged(nameof(EditStoreNameText));
        }
    }

    /// <summary>
    /// Gets or sets the store address text.
    /// </summary>
    public string EditStoreAddressText
    {
        get => _editStoreAddressText;
        set
        {
            _editStoreAddressText = value;
            OnPropertyChanged(nameof(EditStoreAddressText));
        }
    }

    /// <summary>
    /// Gets or sets the store email text.
    /// </summary>
    public string EditStoreEmailText
    {
        get => _editStoreEmailText;
        set
        {
            _editStoreEmailText = value;
            OnPropertyChanged(nameof(EditStoreEmailText));
        }
    }

    /// <summary>
    /// Gets or sets the store telephone text.
    /// </summary>
    public string EditStoreTelText
    {
        get => _editStoreTelText;
        set
        {
            _editStoreTelText = value;
            OnPropertyChanged(nameof(EditStoreTelText));
        }
    }

    public string OperatingHoursText
    {
        get => _operatingHoursText;
        set
        {
            _operatingHoursText = value;
            OnPropertyChanged(nameof(OperatingHoursText));
        }
    }
}
