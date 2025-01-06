using System.ComponentModel;
using System.Text.RegularExpressions;

using Microsoft.UI.Xaml.Controls;

using SipPOS.Resources.Controls;
using SipPOS.Models.Entity;
using SipPOS.DataTransfer.General;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.General.Implementations;
using SipPOS.Services.Configuration.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Context.Configuration.Interfaces;

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

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Handles the modification of editable text fields and enables the save button if changes are detected.
    /// </summary>
    /// <param name="saveChangesOnStoreConfigurationButton">The button to enable if changes are detected.</param>
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

    /// <summary>
    /// Handles the change of the opening hour time picker and updates the operating hours text.
    /// </summary>
    /// <param name="newOpeningTime">The new opening time.</param>
    /// <param name="saveChangesOnStoreConfigurationButton">The button to enable if changes are detected.</param>
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

    /// <summary>
    /// Handles the change of the closing hour time picker and updates the operating hours text.
    /// </summary>
    /// <param name="newClosingTime">The new closing time.</param>
    /// <param name="saveChangesOnStoreConfigurationButton">The button to enable if changes are detected.</param>
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

    /// <summary>
    /// Handles the save button click event to save changes to the store configuration.
    /// </summary>
    /// <param name="editStoreConfigurationResultContentDialog">The content dialog to show the result of the save operation.</param>
    /// <param name="saveChangesOnStoreConfigurationButton">The button to disable after saving changes.</param>
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

    /// <summary>
    /// Handles the cancel button click event to reset changes to the store configuration.
    /// </summary>
    /// <param name="saveChangesOnStoreConfigurationButton">The button to disable after canceling changes.</param>
    public void HandleCancelChangesOnStoreConfigurationButtonClick(Button saveChangesOnStoreConfigurationButton)
    {
        resetToCurrentStoreConfiguration();

        saveChangesOnStoreConfigurationButton.IsEnabled = false;
    }

    /// <summary>
    /// Handles the save click event for the store name editable text field.
    /// </summary>
    /// <param name="storeNameErrorMessageTeachingTip">The teaching tip to show error messages.</param>
    /// <param name="storeNameEditableTextField">The editable text field for the store name.</param>
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

    /// <summary>
    /// Handles the save click event for the store address editable text field.
    /// </summary>
    /// <param name="storeAddressErrorMessageTeachingTip">The teaching tip to show error messages.</param>
    /// <param name="storeAddressEditableTextField">The editable text field for the store address.</param>
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

    /// <summary>
    /// Handles the save click event for the store email editable text field.
    /// </summary>
    /// <param name="storeEmailErrorMessageTeachingTip">The teaching tip to show error messages.</param>
    /// <param name="storeEmailEditableTextField">The editable text field for the store email.</param>
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

    /// <summary>
    /// Handles the save click event for the store telephone editable text field.
    /// </summary>
    /// <param name="storeTelErrorMessageTeachingTip">The teaching tip to show error messages.</param>
    /// <param name="storeTelEditableTextField">The editable text field for the store telephone.</param>
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

    /// <summary>
    /// Resets the editable fields to the current store configuration.
    /// </summary>
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

    /// <summary>
    /// Validates the operating hours to ensure the opening time is before the closing time.
    /// </summary>
    /// <returns>True if the operating hours are valid, otherwise false.</returns>
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

    /// <summary>
    /// Shows a content dialog with the specified message.
    /// </summary>
    /// <param name="message">The message to display in the content dialog.</param>
    /// <param name="editStoreConfigurationContentDialog">The content dialog to show the message.</param>
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

    /// <summary>
    /// Gets or sets the store opening time.
    /// </summary>
    public TimeSpan EditOpeningTime
    {
        get => _editOpeningTime;
        set
        {
            _editOpeningTime = value;
            OnPropertyChanged(nameof(EditOpeningTime));
        }
    }

    /// <summary>
    /// Gets or sets the store closing time.
    /// </summary>
    public TimeSpan EditClosingTime
    {
        get => _editClosingTime;
        set
        {
            _editClosingTime = value;
            OnPropertyChanged(nameof(EditClosingTime));
        }
    }

    /// <summary>
    /// Gets or sets the opacity of the operating hours error message.
    /// </summary>
    public float EditOperatingHoursErrorMessageOpacity
    {
        get => _editOperatingHoursErrorMessageOpacity;
        set
        {
            _editOperatingHoursErrorMessageOpacity = value;
            OnPropertyChanged(nameof(EditOperatingHoursErrorMessageOpacity));
        }
    }

    /// <summary>
    /// Gets or sets the text of the operating hours error message.
    /// </summary>
    public string EditOperatingHoursErrorMessageText
    {
        get => _editOperatingHoursErrorMessageText;
        set
        {
            _editOperatingHoursErrorMessageText = value;
            OnPropertyChanged(nameof(EditOperatingHoursErrorMessageText));
        }
    }

    /// <summary>
    /// Gets or sets the operating hours text.
    /// </summary>
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
