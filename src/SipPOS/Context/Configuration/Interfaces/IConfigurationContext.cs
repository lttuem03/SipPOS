namespace SipPOS.Context.Configuration.Interfaces;

/// <summary>
/// Defines methods to manage the configuration context.
/// </summary>
public interface IConfigurationContext
{
    /// <summary>
    /// Gets the current configuration.
    /// </summary>
    /// <returns>The current configuration if set; otherwise, null.</returns>
    Models.General.Configuration? GetConfiguration();

    /// <summary>
    /// Determines whether the configuration is set.
    /// </summary>
    /// <returns><c>true</c> if the configuration is set; otherwise, <c>false</c>.</returns>
    bool IsSet();

    /// <summary>
    /// Sets the configuration.
    /// </summary>
    /// <param name="configuration">The configuration to set.</param>
    void SetConfiguration(Models.General.Configuration configuration);

    /// <summary>
    /// Clears the current configuration.
    /// </summary>
    void ClearConfiguration();
}
