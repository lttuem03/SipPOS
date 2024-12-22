using SipPOS.Context.Configuration.Interfaces;
using SipPOS.DataTransfer.General;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Configuration.Interfaces;

namespace SipPOS.Services.Configuration.Implementations;

/// <summary>
/// Service for managing configuration settings.
/// </summary>
public class ConfigurationService : IConfigurationService
{
    /// <summary>
    /// Loads the configuration asynchronously for the specified store ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task LoadAsync(long storeId)
    {
        var configurationDao = App.GetService<IConfigurationDao>();
        var configurationDto = await configurationDao.GetByIdAsync(storeId);

        if (configurationDto == null)
            return;

        var configurationContext = App.GetService<IConfigurationContext>();

        configurationContext.SetConfiguration(new Models.General.Configuration(storeId, configurationDto));

        // TODO: Move this to the "Salary approval" feature

        // Check if today is the start of the new salary cycle
        // if it is, change the "Current" salary values to the
        // "Next" salary values, because there might have been
        // changes made in the last month to the salary
        // configuration
        var today = DateOnly.FromDateTime(DateTime.Today);
        var updateConfigurationDto = new ConfigurationDto();

        if (today.Day == 1)
        {
            updateConfigurationDto.CurrentStaffBaseSalary = configurationDto.NextStaffBaseSalary;
            updateConfigurationDto.CurrentStaffHourlySalary = configurationDto.NextStaffHourlySalary;
            updateConfigurationDto.CurrentAssistantManagerBaseSalary = configurationDto.NextAssistantManagerBaseSalary;
            updateConfigurationDto.CurrentAssistantManagerHourlySalary = configurationDto.NextAssistantManagerHourlySalary;
            updateConfigurationDto.CurrentStoreManagerBaseSalary = configurationDto.NextStoreManagerBaseSalary;
            updateConfigurationDto.CurrentStoreManagerHourlySalary = configurationDto.NextStoreManagerHourlySalary;
        }

        // You might ask: What if the user doesn't open the app on the first day of the month?
        // Good question, the answer is: YES

        // Actually, the definitive way to know if the user (most likely the store manager)
        // has resetted the salary cycle depends on the feature "Salary approval", which
        // is currently not implemented.

        await UpdateAsync(updateConfigurationDto);

        // Reload the configuration to reflect the changes
        configurationDto = await configurationDao.GetByIdAsync(storeId);

        if (configurationDto == null)
            return;

        configurationContext.ClearConfiguration();
        configurationContext.SetConfiguration(new Models.General.Configuration(storeId, configurationDto));
    }

    /// <summary>
    /// Creates a new configuration asynchronously for the specified store ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="configurationDto">The configuration data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the creation was successful.</returns>
    public async Task<bool> CreateAsync(long storeId, ConfigurationDto configurationDto)
    {
        var configurationDao = App.GetService<IConfigurationDao>();
        var insertResult = await configurationDao.InsertAsync(storeId, configurationDto);

        if (insertResult == null)
            return false;

        return true;
    }

    /// <summary>
    /// Updates the current configuration asynchronously with the specified parameters.
    /// </summary>
    /// <param name="configuredParameters">The configured parameters to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the update was successful.</returns>
    public async Task<bool> UpdateAsync(ConfigurationDto configuredParameters)
    {
        // Requires being logged in, and there is current Configuration set
        var configurationContext = App.GetService<IConfigurationContext>();

        if (!configurationContext.IsSet())
            return false;

        var currentConfiguration = configurationContext.GetConfiguration();

        if (currentConfiguration == null)
            return false;

        var updatedConfigurationDto = new ConfigurationDto()
        {
            StoreId = currentConfiguration.StoreId,

            OpeningTime = (configuredParameters.OpeningTime != TimeOnly.MinValue) ?
                configuredParameters.OpeningTime : currentConfiguration.OpeningTime,
            ClosingTime = (configuredParameters.ClosingTime != TimeOnly.MinValue) ?
                configuredParameters.ClosingTime : currentConfiguration.ClosingTime,
            TaxCode = (configuredParameters.TaxCode != string.Empty) ?
                configuredParameters.TaxCode : currentConfiguration.TaxCode,
            VatRate = (configuredParameters.VatRate != -1.0m) ?
                configuredParameters.VatRate : currentConfiguration.VatRate,
            VatMethod = (configuredParameters.VatMethod != string.Empty) ?
                configuredParameters.VatMethod : currentConfiguration.VatMethod,

            CurrentStaffBaseSalary = (configuredParameters.CurrentStaffBaseSalary != -1m) ?
                configuredParameters.CurrentStaffBaseSalary : currentConfiguration.CurrentStaffBaseSalary,
            CurrentStaffHourlySalary = (configuredParameters.CurrentStaffHourlySalary != -1m) ?
                configuredParameters.CurrentStaffHourlySalary : currentConfiguration.CurrentStaffHourlySalary,
            CurrentAssistantManagerBaseSalary = (configuredParameters.CurrentAssistantManagerBaseSalary != -1m) ?
                configuredParameters.CurrentAssistantManagerBaseSalary : currentConfiguration.CurrentAssistantManagerBaseSalary,
            CurrentAssistantManagerHourlySalary = (configuredParameters.CurrentAssistantManagerHourlySalary != -1m) ?
                configuredParameters.CurrentAssistantManagerHourlySalary : currentConfiguration.CurrentAssistantManagerHourlySalary,
            CurrentStoreManagerBaseSalary = (configuredParameters.CurrentStoreManagerBaseSalary != -1m) ?
                configuredParameters.CurrentStoreManagerBaseSalary : currentConfiguration.CurrentStoreManagerBaseSalary,
            CurrentStoreManagerHourlySalary = (configuredParameters.CurrentStoreManagerHourlySalary != -1m) ?
                configuredParameters.CurrentStoreManagerHourlySalary : currentConfiguration.CurrentStoreManagerHourlySalary,

            NextStaffBaseSalary = (configuredParameters.NextStaffBaseSalary != -1m) ?
                configuredParameters.NextStaffBaseSalary : currentConfiguration.NextStaffBaseSalary,
            NextStaffHourlySalary = (configuredParameters.NextStaffHourlySalary != -1m) ?
                configuredParameters.NextStaffHourlySalary : currentConfiguration.NextStaffHourlySalary,
            NextAssistantManagerBaseSalary = (configuredParameters.NextAssistantManagerBaseSalary != -1m) ?
                configuredParameters.NextAssistantManagerBaseSalary : currentConfiguration.NextAssistantManagerBaseSalary,
            NextAssistantManagerHourlySalary = (configuredParameters.NextAssistantManagerHourlySalary != -1m) ?
                configuredParameters.NextAssistantManagerHourlySalary : currentConfiguration.NextAssistantManagerHourlySalary,
            NextStoreManagerBaseSalary = (configuredParameters.NextStoreManagerBaseSalary != -1m) ?
                configuredParameters.NextStoreManagerBaseSalary : currentConfiguration.NextStoreManagerBaseSalary,
            NextStoreManagerHourlySalary = (configuredParameters.NextStoreManagerHourlySalary != -1m) ?
                configuredParameters.NextStoreManagerHourlySalary : currentConfiguration.NextStoreManagerHourlySalary
        };

        var configurationDao = App.GetService<IConfigurationDao>();

        var updateResult = await configurationDao.UpdateByIdAsync(currentConfiguration.StoreId, updatedConfigurationDto);

        if (updateResult == null)
            return false;

        return true;
    }
}
