using System.ComponentModel;
using System.Text.RegularExpressions;

using Microsoft.UI.Xaml.Controls;

using SipPOS.Context.Configuration.Interfaces;

using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Resources.Controls;
using SipPOS.Models.Entity;
using SipPOS.Services.Configuration.Interfaces;
using SipPOS.DataTransfer.General;
using SipPOS.Services.DataAccess.Interfaces;
using Windows.Media.Capture;

namespace SipPOS.ViewModels.Configuration;

/// <summary>
/// ViewModel for store configuration.
/// </summary>
public class StoreConfigurationViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    // Contextual properties
    private Store? _currentStore;
    private Models.General.Configuration? _currentConfiguration;

    // Data-bound properties
    private string _editStoreNameText = string.Empty;
    private string _editStoreAddressText = string.Empty;
    private string _editStoreEmailText = string.Empty;
    private string _editStoreTelText = string.Empty;
    private TimeSpan _editOpeningTime = TimeSpan.MinValue;
    private TimeSpan _editClosingTime = TimeSpan.MinValue;
    private string _operatingHoursText = string.Empty;
    private float _editOperatingHoursErrorMessageOpacity = 0.0F;
    private string _editOperatingHoursErrorMessageText = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="StoreConfigurationViewModel"/> class.
    /// </summary>
    public StoreConfigurationViewModel()
    {
        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
            return;

        var configurationContext = App.GetService<IConfigurationContext>();

        if (configurationContext == null)
            return;

        _currentConfiguration = configurationContext.GetConfiguration();

        if (_currentConfiguration == null)
            return;

        _currentStore = storeAuthenticationService.Context.CurrentStore;

        if (_currentStore == null)
            return;

        EditStoreNameText = _currentStore.Name;
        EditStoreAddressText = _currentStore.Address;
        EditStoreEmailText = _currentStore.Email;
        EditStoreTelText = _currentStore.Tel;

        EditOpeningTime = _currentConfiguration.OpeningTime.ToTimeSpan();
        EditClosingTime = _currentConfiguration.ClosingTime.ToTimeSpan();

        OperatingHoursText = $"{_currentConfiguration.OpeningTime.ToString("HH:mm")} đến {_currentConfiguration.ClosingTime.ToString("HH:mm")}";
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

    public void HandleEditOpeningHourTimePickerSelectedTimeChanged(TimeSpan newOpeningTime, Button saveChangesOnStoreConfigurationButton)
    {
        if (_currentConfiguration == null)
            return;

        if (newOpeningTime == _currentConfiguration.OpeningTime.ToTimeSpan())
            return;

        EditOpeningTime = newOpeningTime;
        OperatingHoursText = $"{TimeOnly.FromTimeSpan(EditOpeningTime).ToString("HH:mm")} đến {TimeOnly.FromTimeSpan(EditClosingTime).ToString("HH:mm")}";

        saveChangesOnStoreConfigurationButton.IsEnabled = true;
    }

    public void HandleEditClosingHourTimePickerSelectedTimeChanged(TimeSpan newClosingTime, Button saveChangesOnStoreConfigurationButton)
    {
        if (_currentConfiguration == null)
            return;

        if (newClosingTime == _currentConfiguration.ClosingTime.ToTimeSpan())
            return;

        EditClosingTime = newClosingTime;
        OperatingHoursText = $"{TimeOnly.FromTimeSpan(EditOpeningTime).ToString("HH:mm")} đến {TimeOnly.FromTimeSpan(EditClosingTime).ToString("HH:mm")}";

        saveChangesOnStoreConfigurationButton.IsEnabled = true;
    }

    public async void HandleSaveChangesOnStoreConfigurationButtonClick(ContentDialog editStoreConfigurationResultContentDialog, Button saveChangesOnStoreConfigurationButton)
    {
        // Other validations is already done in "SaveClicked" event handlers

        if (validateOperatingHours() == false)
            return;

        if (_currentStore == null || _currentConfiguration == null)
            return;

        // Update store information
        var storeDao = App.GetService<IStoreDao>();
        var currentStoreDto = await storeDao.GetByIdAsync(_currentStore.Id);

        if (currentStoreDto == null)
        {
            resetToCurrentStoreConfiguration();
            saveChangesOnStoreConfigurationButton.IsEnabled = false;
            showResultContentDialog("Cập nhật thiết lập cửa hàng thất bại", editStoreConfigurationResultContentDialog);
            return;
        }

        currentStoreDto.Name = EditStoreNameText;
        currentStoreDto.Address = EditStoreAddressText;
        currentStoreDto.Email = EditStoreEmailText;
        currentStoreDto.Tel = EditStoreTelText;

        var updatedStoreDto = await storeDao.UpdateByIdAsync(_currentStore.Id, currentStoreDto);

        if (updatedStoreDto == null)
        {
            resetToCurrentStoreConfiguration();
            saveChangesOnStoreConfigurationButton.IsEnabled = false;
            showResultContentDialog("Cập nhật thiết lập cửa hàng thất bại", editStoreConfigurationResultContentDialog);
            return;
        }

        // Update operating hours
        var configurationService = App.GetService<IConfigurationService>();

        var result = await configurationService.UpdateAsync(new ConfigurationDto
        {
            OpeningTime = TimeOnly.FromTimeSpan(EditOpeningTime),
            ClosingTime = TimeOnly.FromTimeSpan(EditClosingTime),
        });

        if (!result)
        {
            resetToCurrentStoreConfiguration();
            saveChangesOnStoreConfigurationButton.IsEnabled = false;
            showResultContentDialog("Cập nhật thiết lập cửa hàng thất bại", editStoreConfigurationResultContentDialog);
            return;
        }

        // SUCCESSFUL, RELOAD CONTENTS FOR CLARITY
        if (App.GetService<IStoreAuthenticationService>() is not StoreAuthenticationService storeAuthenticationService)
            return;

        var currentStoreId = storeAuthenticationService.GetCurrentStoreId();

        storeAuthenticationService.Context.ClearStore();
        storeAuthenticationService.Context.SetStore(new Store(currentStoreId, updatedStoreDto));
        
        _currentStore = storeAuthenticationService.Context.CurrentStore;

        await configurationService.LoadAsync(currentStoreId);
        _currentConfiguration = App.GetService<IConfigurationContext>().GetConfiguration();

        // Again, pray to god that this will not happen
        if (_currentStore == null || _currentConfiguration == null)
            return;

        EditStoreNameText = _currentStore.Name;
        EditStoreAddressText = _currentStore.Address;
        EditStoreEmailText = _currentStore.Email;
        EditStoreTelText = _currentStore.Tel;

        EditOpeningTime = _currentConfiguration.OpeningTime.ToTimeSpan();
        EditClosingTime = _currentConfiguration.ClosingTime.ToTimeSpan();

        OperatingHoursText = $"{_currentConfiguration.OpeningTime.ToString("HH:mm")} đến {_currentConfiguration.ClosingTime.ToString("HH:mm")}";

        saveChangesOnStoreConfigurationButton.IsEnabled = false;
        showResultContentDialog("Cập nhật thiết lập cửa hàng thành công", editStoreConfigurationResultContentDialog);
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

        EditOpeningTime = _currentConfiguration == null ? TimeSpan.MinValue : _currentConfiguration.OpeningTime.ToTimeSpan();
        EditClosingTime = _currentConfiguration == null ? TimeSpan.MinValue : _currentConfiguration.ClosingTime.ToTimeSpan();

        OperatingHoursText = $"{TimeOnly.FromTimeSpan(EditOpeningTime).ToString("HH:mm")} đến {TimeOnly.FromTimeSpan(EditClosingTime).ToString("HH:mm")}";

        EditOperatingHoursErrorMessageOpacity = 0.0F;
    }

    private bool validateOperatingHours()
    {
        var allFieldsValid = true;

        if (EditOpeningTime.CompareTo(EditClosingTime) >= 0)
        {
            EditOperatingHoursErrorMessageText = "Giờ mở cửa phải trước giờ đóng cửa";
            EditOperatingHoursErrorMessageOpacity = 1.0F;

            allFieldsValid = false;

            return allFieldsValid;
        }

        if (allFieldsValid)
            EditOperatingHoursErrorMessageOpacity = 0.0F;

        return allFieldsValid;
    }

    private async void showResultContentDialog(string message, ContentDialog editStoreConfigurationContentDialog)
    {
        editStoreConfigurationContentDialog.Content = message;

        _ = await editStoreConfigurationContentDialog.ShowAsync();
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

    public TimeSpan EditOpeningTime
    {
        get => _editOpeningTime;
        set
        {
            _editOpeningTime = value;
            OnPropertyChanged(nameof(EditOpeningTime));
        }
    }

    public TimeSpan EditClosingTime
    {
        get => _editClosingTime;
        set
        {
            _editClosingTime = value;
            OnPropertyChanged(nameof(EditClosingTime));
        }
    }

    public float EditOperatingHoursErrorMessageOpacity
    {
        get => _editOperatingHoursErrorMessageOpacity;
        set
        {
            _editOperatingHoursErrorMessageOpacity = value;
            OnPropertyChanged(nameof(EditOperatingHoursErrorMessageOpacity));
        }
    }

    public string EditOperatingHoursErrorMessageText
    {
        get => _editOperatingHoursErrorMessageText;
        set
        {
            _editOperatingHoursErrorMessageText = value;
            OnPropertyChanged(nameof(EditOperatingHoursErrorMessageText));
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
