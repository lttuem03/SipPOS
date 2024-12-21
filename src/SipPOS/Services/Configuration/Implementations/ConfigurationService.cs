using SipPOS.Context.Configuration.Interfaces;
using SipPOS.DataTransfer.General;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.Configuration.Interfaces;

namespace SipPOS.Services.Configuration.Implementations;

public class ConfigurationService : IConfigurationService
{
    public async Task LoadAsync(long storeId)
    {
        var configurationDao = App.GetService<IConfigurationDao>();
        var configurationDto = await configurationDao.GetByIdAsync(storeId);

        if (configurationDto == null)
            return;

        var configurationContext = App.GetService<IConfigurationContext>();
        configurationContext.SetConfiguration(new Models.General.Configuration(storeId, configurationDto));
    }

    public async Task<bool> CreateAsync(long storeId, ConfigurationDto configurationDto)
    {
        var configurationDao = App.GetService<IConfigurationDao>();
        var insertResult = await configurationDao.InsertAsync(storeId, configurationDto);

        if (insertResult == null)
            return false;

        return true;
    }

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
            StaffBaseSalary = (configuredParameters.StaffBaseSalary != -1m) ?
                configuredParameters.StaffBaseSalary : currentConfiguration.StaffBaseSalary,
            StaffHourlySalary = (configuredParameters.StaffHourlySalary != -1m) ?
                configuredParameters.StaffHourlySalary : currentConfiguration.StaffHourlySalary,
            AssistantManagerBaseSalary = (configuredParameters.AssistantManagerBaseSalary != -1m) ?
                configuredParameters.AssistantManagerBaseSalary : currentConfiguration.AssistantManagerBaseSalary,
            AssistantManagerHourlySalary = (configuredParameters.AssistantManagerHourlySalary != -1m) ?
                configuredParameters.AssistantManagerHourlySalary : currentConfiguration.AssistantManagerHourlySalary,
            StoreManagerBaseSalary = (configuredParameters.StoreManagerBaseSalary != -1m) ?
                configuredParameters.StoreManagerBaseSalary : currentConfiguration.StoreManagerBaseSalary,
            StoreManagerHourlySalary = (configuredParameters.StoreManagerHourlySalary != -1m) ?
                configuredParameters.StoreManagerHourlySalary : currentConfiguration.StoreManagerHourlySalary
        };

        var configurationDao = App.GetService<IConfigurationDao>();

        var updateResult = await configurationDao.UpdateByIdAsync(currentConfiguration.StoreId, updatedConfigurationDto);

        if (updateResult == null)
            return false;

        return true;
    }
}