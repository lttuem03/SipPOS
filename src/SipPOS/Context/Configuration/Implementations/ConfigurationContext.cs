using SipPOS.Context.Configuration.Interfaces;

namespace SipPOS.Context.Configuration.Implementations;

/// <summary>
/// Provides methods to manage the configuration context.
/// </summary>
public class ConfigurationContext : IConfigurationContext
{
    /// <summary>
    /// Gets or sets the current configuration.
    /// </summary>
    public Models.General.Configuration? CurrentConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationContext"/> class.
    /// </summary>
    public ConfigurationContext()
    {
        CurrentConfiguration = null;
    }

    /// <summary>
    /// Gets the current configuration.
    /// </summary>
    /// <returns>The current configuration if set; otherwise, null.</returns>
    public Models.General.Configuration? GetConfiguration()
    {
        return CurrentConfiguration;
    }

    /// <summary>
    /// Determines whether the configuration is set.
    /// </summary>
    /// <returns><c>true</c> if the configuration is set; otherwise, <c>false</c>.</returns>
    public bool IsSet()
    {
        return (CurrentConfiguration != null);
    }

    /// <summary>
    /// Sets the configuration.
    /// </summary>
    /// <param name="configuration">The configuration to set.</param>
    public void SetConfiguration(Models.General.Configuration configuration)
    {
        CurrentConfiguration = configuration;
    }

    /// <summary>
    /// Clears the current configuration.
    /// </summary>
    public void ClearConfiguration()
    {
        CurrentConfiguration = null;
    }
}
