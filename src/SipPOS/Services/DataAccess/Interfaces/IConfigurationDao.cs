using SipPOS.DataTransfer.General;

namespace SipPOS.Services.DataAccess.Interfaces;

/// <summary>
/// Interface for managing configuration data access operations.
/// </summary>
public interface IConfigurationDao
{
    /// <summary>
    /// Inserts a new configuration into the database.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="configurationDto">The configuration data transfer object to insert.</param>
    /// <returns>The inserted configuration, or null if the insertion failed.</returns>
    Task<ConfigurationDto?> InsertAsync(long storeId, ConfigurationDto configurationDto);

    /// <summary>
    /// Retrieves a configuration by its store ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <returns>The configuration, or null if not found.</returns>
    Task<ConfigurationDto?> GetByIdAsync(long storeId);

    /// <summary>
    /// Updates a configuration by its store ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="updatedConfigurationDto">The updated configuration data transfer object.</param>
    /// <returns>The updated configuration, or null if the update failed.</returns>
    Task<ConfigurationDto?> UpdateByIdAsync(long storeId, ConfigurationDto updatedConfigurationDto);
}
