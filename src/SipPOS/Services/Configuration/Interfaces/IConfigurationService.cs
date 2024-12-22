using SipPOS.DataTransfer.General;

namespace SipPOS.Services.Configuration.Interfaces;

public interface IConfigurationService
{
    // The ConfigurationService works alongside the ConfigurationContext, but do not
    // contains it in like the AuthenticationService and AuthenticationContext.

    /// <summary>
    /// Loads the configuration for the specified store.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task LoadAsync(long storeId);

    /// <summary>
    /// Creates a new configuration for the specified store.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="configurationDto">The configuration data transfer object.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
    Task<bool> CreateAsync(long storeId, ConfigurationDto configurationDto);

    // The UpdateAsync only needs the parameters in the Dto that is different from their
    // initialization, that way, it'll know which configuration to update.
    // Meaning you don't need to copy ALL the parameters from the current Configuration
    // to the configuredParameters parameter.

    /// <summary>
    /// Updates the configuration with the specified parameters.
    /// </summary>
    /// <param name="configuredParameters">The configuration parameters to update.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating success or failure.</returns>
    /// <remarks>The UpdateAsync only needs the parameters in the Dto that is different from their
    /// initialization, that way, it'll know which configuration to update. 
    /// Meaning you don't need to copy ALL the parameters from the current Configuration
    /// to the configuredParameters parameter.</remarks>
    Task<bool> UpdateAsync(ConfigurationDto configuredParameters);
}