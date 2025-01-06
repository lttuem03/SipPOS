using System.ComponentModel;
using System.Text.RegularExpressions;

using Microsoft.UI.Xaml.Controls;

using SipPOS.DataTransfer.General;
using SipPOS.Resources.Controls;
using SipPOS.Services.Configuration.Interfaces;
using SipPOS.Services.General.Interfaces;
using SipPOS.Context.Configuration.Interfaces;

namespace SipPOS.ViewModels.Configuration;

/// <summary>
/// ViewModel for managing tax configuration settings.
/// </summary>
public class TaxConfigurationViewModel : INotifyPropertyChanged
{
    private Models.General.Configuration? _currentConfiguration;

    private string _editTaxCode = string.Empty;
    private decimal _editVatRate = -1.0m;
    private string _editVatMethod = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event for the specified property.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TaxConfigurationViewModel"/> class.
    /// Loads the current tax configuration settings from the configuration context.
    /// </summary>
    public TaxConfigurationViewModel()
    {
        var configurationContext = App.GetService<IConfigurationContext>();

        if (configurationContext == null)
            return;

        _currentConfiguration = configurationContext.GetConfiguration();

        if (_currentConfiguration == null)
            return;

        EditTaxCode = _currentConfiguration.TaxCode;
        EditVatRate = _currentConfiguration.VatRate;
        EditVatMethod = _currentConfiguration.VatMethod;
    }

    /// <summary>
    /// Handles the save click event for the tax configuration.
    /// </summary>
    /// <param name="editStoreConfigurationResultContentDialog">The content dialog to show the result message.</param>
    /// <param name="saveChangesOnTaxConfigurationButton">The button to save changes on tax configuration.</param>
    public async void HandleSaveChangesOnTaxConfigurationButtonClick(ContentDialog editStoreConfigurationResultContentDialog, Button saveChangesOnTaxConfigurationButton)
    {
        if (_currentConfiguration == null)
            return;

        var configurationService = App.GetService<IConfigurationService>();

        var result = await configurationService.UpdateAsync(new ConfigurationDto
        {
            TaxCode = EditTaxCode,
            VatRate = EditVatRate,
            VatMethod = EditVatMethod
        });

        if (!result)
        {
            resetToCurrentTaxConfiguration();
            showResultContentDialog("Cập nhật thiết lập thuế thất bại", editStoreConfigurationResultContentDialog);
            return;
        }

        // SUCCESSFUL, RELOAD CONTENTS FOR CLARITY
        var currentStoreId = App.GetService<IStoreAuthenticationService>().GetCurrentStoreId();

        await configurationService.LoadAsync(currentStoreId);
        _currentConfiguration = App.GetService<IConfigurationContext>().GetConfiguration();

        // Again, pray to god that this will not happen
        if (_currentConfiguration == null)
            return;

        EditTaxCode = _currentConfiguration.TaxCode;
        EditVatRate = _currentConfiguration.VatRate;
        EditVatMethod = _currentConfiguration.VatMethod;

        saveChangesOnTaxConfigurationButton.IsEnabled = false;
        showResultContentDialog("Cập nhật thiết lập thuế thành công", editStoreConfigurationResultContentDialog);
    }

    /// <summary>
    /// Handles the cancel click event for the tax configuration.
    /// </summary>
    /// <param name="saveChangesOnTaxConfigurationButton">The button to save changes on tax configuration.</param>
    public void HandleCancelChangesOnTaxConfigurationButtonClick(Button saveChangesOnTaxConfigurationButton)
    {
        resetToCurrentTaxConfiguration();

        saveChangesOnTaxConfigurationButton.IsEnabled = false;
    }

    /// <summary>
    /// Handles the text modified event for the tax code editable text field.
    /// </summary>
    /// <param name="saveChangesOnTaxConfigurationButton">The button to save changes on tax configuration.</param>
    public void HandleTaxCodeEditableTextFieldTextModified(Button saveChangesOnTaxConfigurationButton)
    {
        if (_currentConfiguration == null)
            return;

        if (EditTaxCode != _currentConfiguration.TaxCode)
        {
            saveChangesOnTaxConfigurationButton.IsEnabled = true;
        }
    }

    /// <summary>
    /// Handles the save click event for the tax code editable text field.
    /// </summary>
    /// <param name="taxCodeErrorMessageTeachingTip">The teaching tip to show error messages.</param>
    /// <param name="taxCodeEditableTextField">The editable text field for the tax code.</param>
    public void HandleTaxCodeEditableTextField_SaveClicked
    (
        TeachingTip taxCodeErrorMessageTeachingTip,
        EditableTextField taxCodeEditableTextField
    )
    {
        if (string.IsNullOrEmpty(EditTaxCode))
        {
            taxCodeErrorMessageTeachingTip.Subtitle = "Mã số thuế không được rỗng";
            taxCodeErrorMessageTeachingTip.IsOpen = true;
            taxCodeEditableTextField.ResetState();
            EditTaxCode = _currentConfiguration == null ? "Lỗi tải thiết lập" : _currentConfiguration.TaxCode;
            return;
        }

        var taxCodePattern = @"^\d{8}[1-9]\d(-\d\d[1-9])?$";

        if (!Regex.IsMatch(EditTaxCode, taxCodePattern))
        {
            taxCodeErrorMessageTeachingTip.Subtitle = "Mã số thuế sai địng dạng (có dấu '-' nếu MST có 13 chữ số)";
            taxCodeErrorMessageTeachingTip.IsOpen = true;
            taxCodeEditableTextField.ResetState();
            EditTaxCode = _currentConfiguration == null ? "Lỗi tải thiết lập" : _currentConfiguration.TaxCode;
            return;
        }
    }

    /// <summary>
    /// Handles the selection changed event for the VAT rate combo box.
    /// </summary>
    /// <param name="selectedIndex">The selected index of the combo box.</param>
    /// <param name="saveChangesOnTaxConfigurationButton">The button to save changes on tax configuration.</param>
    public void HandleGeneralVatRateComboBoxSelectionChanged(int selectedIndex, Button saveChangesOnTaxConfigurationButton)
    {
        var newVatRate = selectedIndex switch
        {
            0 => 0.00m,
            1 => 0.01m,
            2 => 0.03m,
            3 => 0.05m,
            4 => 0.08m,
            5 => 0.10m,
            _ => -1.00m
        };

        if (newVatRate != EditVatRate)
        {
            EditVatRate = newVatRate;
            saveChangesOnTaxConfigurationButton.IsEnabled = true;
        }
        else
        {
            saveChangesOnTaxConfigurationButton.IsEnabled = false;
        }
    }

    /// <summary>
    /// Handles the selection changed event for the VAT method combo box.
    /// </summary>
    /// <param name="selectedIndex">The selected index of the combo box.</param>
    /// <param name="saveChangesOnTaxConfigurationButton">The button to save changes on tax configuration.</param>
    public void HandleSelectVatMethodComboBoxSelectionChanged(int selectedIndex, Button saveChangesOnTaxConfigurationButton)
    {
        var newVatMethod = selectedIndex switch
        {
            0 => "VAT_INCLUDED",
            1 => "ORDER_BASED",
            _ => string.Empty
        };

        if (newVatMethod != EditVatMethod)
        {
            EditVatMethod = newVatMethod;
            saveChangesOnTaxConfigurationButton.IsEnabled = true;
        }
        else
        {
            saveChangesOnTaxConfigurationButton.IsEnabled = false;
        }
    }

    /// <summary>
    /// Resets the editable fields to the current tax configuration.
    /// </summary>
    private void resetToCurrentTaxConfiguration()
    {
        EditTaxCode = _currentConfiguration == null ? "Lỗi tải thiết lập" : _currentConfiguration.TaxCode;
        EditVatRate = _currentConfiguration == null ? -1.0m : _currentConfiguration.VatRate;
        EditVatMethod = _currentConfiguration == null ? "Lỗi tải thiết lập" : _currentConfiguration.VatMethod;
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
    /// Gets or sets the tax code.
    /// </summary>
    public string EditTaxCode
    {
        get => _editTaxCode;
        set
        {
            _editTaxCode = value;
            OnPropertyChanged(nameof(EditTaxCode));
        }
    }

    /// <summary>
    /// Gets or sets the VAT rate.
    /// </summary>
    public decimal EditVatRate
    {
        get => _editVatRate;
        set
        {
            _editVatRate = value;
            OnPropertyChanged(nameof(EditVatRate));
        }
    }

    /// <summary>
    /// Gets or sets the VAT method.
    /// </summary>
    public string EditVatMethod
    {
        get => _editVatMethod;
        set
        {
            _editVatMethod = value;
            OnPropertyChanged(nameof(EditVatMethod));
        }
    }
}
