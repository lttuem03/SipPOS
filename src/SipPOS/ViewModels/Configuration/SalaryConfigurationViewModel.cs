using System.ComponentModel;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using SipPOS.DataTransfer.General;
using SipPOS.Context.Configuration.Interfaces;
using SipPOS.Services.Configuration.Interfaces;
using SipPOS.Services.General.Interfaces;

namespace SipPOS.ViewModels.Configuration;

/// <summary>
/// ViewModel for managing salary configuration.
/// </summary>
public class SalaryConfigurationViewModel : INotifyPropertyChanged
{
    /// <summary>
    /// Gets the current configuration.
    /// </summary>
    public Models.General.Configuration? CurrentConfiguration { get; private set; }

    /// <summary>
    /// Gets the DTO for editing salary configuration.
    /// </summary>
    public ConfigurationDto EditSalaryConfigurationDto { get; private set; }

    private string _currentCycleText = string.Empty;
    private string _nextCycleText = string.Empty;

    /// <summary>
    /// Event triggered when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the PropertyChanged event.
    /// </summary>
    /// <param name="propertyName">The name of the property that changed.</param>
    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalaryConfigurationViewModel"/> class.
    /// </summary>
    public SalaryConfigurationViewModel()
    {
        EditSalaryConfigurationDto = new();
        CurrentConfiguration = App.GetService<IConfigurationContext>().GetConfiguration();

        if (CurrentConfiguration == null)
            return;

        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        var currentCycleStart = new DateOnly(currentDate.Year, currentDate.Month, 1);
        var nextCycleStart = currentCycleStart.AddMonths(1);

        CurrentCycleText = $"{currentCycleStart.ToString("dd/MM/yyyy")} - {nextCycleStart.AddDays(-1).ToString("dd/MM/yyyy")}";
        NextCycleText = $"{nextCycleStart.ToString("dd/MM/yyyy")} - {nextCycleStart.AddMonths(1).AddDays(-1).ToString("dd/MM/yyyy")}";
    }

    /// <summary>
    /// Handles the save changes button click event.
    /// </summary>
    /// <param name="unsavedChangesConfigurationDto">The DTO containing unsaved changes.</param>
    /// <param name="unsavedChangesWarningTextBlock">The text block to display unsaved changes warning.</param>
    /// <param name="saveChangesOnSalaryConfigurationButton">The button to save changes.</param>
    public void HandleSaveChangesButtonClick
    (
        ConfigurationDto unsavedChangesConfigurationDto,
        TextBlock unsavedChangesWarningTextBlock,
        Button saveChangesOnSalaryConfigurationButton
    )
    {
        if (CurrentConfiguration == null)
            return;

        if (CurrentConfiguration.NextStaffBaseSalary != unsavedChangesConfigurationDto.NextStaffBaseSalary ||
            CurrentConfiguration.NextStaffHourlySalary != unsavedChangesConfigurationDto.NextStaffHourlySalary ||
            CurrentConfiguration.NextAssistantManagerBaseSalary != unsavedChangesConfigurationDto.NextAssistantManagerBaseSalary ||
            CurrentConfiguration.NextAssistantManagerHourlySalary != unsavedChangesConfigurationDto.NextAssistantManagerHourlySalary ||
            CurrentConfiguration.NextStoreManagerBaseSalary != unsavedChangesConfigurationDto.NextStoreManagerBaseSalary ||
            CurrentConfiguration.NextStoreManagerHourlySalary != unsavedChangesConfigurationDto.NextStoreManagerHourlySalary)
        {
            // Set the new changes as "unsaved changes" in the ViewModel

            EditSalaryConfigurationDto.NextStaffBaseSalary = unsavedChangesConfigurationDto.NextStaffBaseSalary;
            EditSalaryConfigurationDto.NextStaffHourlySalary = unsavedChangesConfigurationDto.NextStaffHourlySalary;
            EditSalaryConfigurationDto.NextAssistantManagerBaseSalary = unsavedChangesConfigurationDto.NextAssistantManagerBaseSalary;
            EditSalaryConfigurationDto.NextAssistantManagerHourlySalary = unsavedChangesConfigurationDto.NextAssistantManagerHourlySalary;
            EditSalaryConfigurationDto.NextStoreManagerBaseSalary = unsavedChangesConfigurationDto.NextStoreManagerBaseSalary;
            EditSalaryConfigurationDto.NextStoreManagerHourlySalary = unsavedChangesConfigurationDto.NextStoreManagerHourlySalary;

            // Don't worry if the value -1m is overwritten, because if it
            // is, it's still be overwritten to the current configuration
            // (or the updated value if there was one or more),
            // and there's no harm in updating the current configuration
            // with the same value.

            // Context: the way ConfigurationService.UpdateAsync(dto)
            // works is that it checks for values in dto that is not
            // its default constructed value and update those ones
            // in the database (other values that was not editted
            // is set to that of the current configuration).

            // Here the "unsaved changes" are set to -1m, so that
            // if there was no changes made to the salary configuration,
            // it will not be updated in UpdateAsync().

            unsavedChangesWarningTextBlock.Visibility = Visibility.Visible;
            saveChangesOnSalaryConfigurationButton.IsEnabled = true;
        }
    }

    /// <summary>
    /// Handles the save changes on salary configuration button click event.
    /// </summary>
    /// <param name="editSalaryConfigurationResultContentDialog">The content dialog to display the result.</param>
    /// <param name="saveChangesOnSalaryConfigurationButton">The button to save changes.</param>
    public async void HandleSaveChangesOnSalaryConfigurationButtonClick
    (
        ContentDialog editSalaryConfigurationResultContentDialog,
        Button saveChangesOnSalaryConfigurationButton
    )
    {
        // Update salary configurations
        var configurationService = App.GetService<IConfigurationService>();

        var result = await configurationService.UpdateAsync(EditSalaryConfigurationDto);

        if (!result)
        {
            // The contents of the salary text boxes stays the same
            // if failed to update it in the database.
            saveChangesOnSalaryConfigurationButton.IsEnabled = false;
            editSalaryConfigurationResultContentDialog.Content = "Cập nhật thiết lập lương thất bại";
            _ = await editSalaryConfigurationResultContentDialog.ShowAsync();
            return;
        }

        // SUCCESSFUL, RELOAD CONTENTS FOR CLARITY
        var currentStoreId = App.GetService<IStoreAuthenticationService>().GetCurrentStoreId();

        await configurationService.LoadAsync(currentStoreId);
        CurrentConfiguration = App.GetService<IConfigurationContext>().GetConfiguration();

        // Again, pray to god that this will not happen
        if (CurrentConfiguration == null)
            return;

        // Reset the "unsaved changes"
        EditSalaryConfigurationDto.NextStaffBaseSalary = -1m;
        EditSalaryConfigurationDto.NextStaffHourlySalary = -1m;
        EditSalaryConfigurationDto.NextAssistantManagerBaseSalary = -1m;
        EditSalaryConfigurationDto.NextAssistantManagerHourlySalary = -1m;
        EditSalaryConfigurationDto.NextStoreManagerBaseSalary = -1m;
        EditSalaryConfigurationDto.NextStoreManagerHourlySalary = -1m;

        // The contents of the salary text boxes are updated in the UI thread
        // after calling this Handle method

        saveChangesOnSalaryConfigurationButton.IsEnabled = false;
        editSalaryConfigurationResultContentDialog.Content = "Cập nhật thiết lập lương thành công";
        _ = await editSalaryConfigurationResultContentDialog.ShowAsync();
    }

    /// <summary>
    /// Handles the cancel changes button click event.
    /// </summary>
    public void HandleCancelChangesOnSalaryConfigurationButtonClick()
    {
        // Re-assigns the salary properties back to their
        // default values so that they won't be updated 
        // when calling ConfigurationService.UpdateAsync()

        EditSalaryConfigurationDto.NextStaffBaseSalary = -1m;
        EditSalaryConfigurationDto.NextStaffHourlySalary = -1m;
        EditSalaryConfigurationDto.NextAssistantManagerBaseSalary = -1m;
        EditSalaryConfigurationDto.NextAssistantManagerHourlySalary = -1m;
        EditSalaryConfigurationDto.NextStoreManagerBaseSalary = -1m;
        EditSalaryConfigurationDto.NextStoreManagerHourlySalary = -1m;
    }

    /// <summary>
    /// Gets or sets the current cycle text.
    /// </summary>
    public string CurrentCycleText
    {
        get => _currentCycleText;
        set
        {
            _currentCycleText = value;
            OnPropertyChanged(nameof(CurrentCycleText));
        }
    }

    /// <summary>
    /// Gets or sets the next cycle text.
    /// </summary>
    public string NextCycleText
    {
        get => _nextCycleText;
        set
        {
            _nextCycleText = value;
            OnPropertyChanged(nameof(NextCycleText));
        }
    }
}
