using SipPOS.Context.Configuration.Interfaces;

namespace SipPOS.Context.Configuration.Implementations;

public class ConfigurationContext : IConfigurationContext
{
    public Models.General.Configuration? CurrentConfiguration;

    public ConfigurationContext()
    {
        CurrentConfiguration = null;
    }

    public Models.General.Configuration? GetConfiguration()
    {
        return CurrentConfiguration;
    }

    public bool IsSet()
    {
        return (CurrentConfiguration != null);
    }

    public void SetConfiguration(Models.General.Configuration configuration)
    {
        CurrentConfiguration = configuration;
    }

    public void ClearConfiguration()
    {
        CurrentConfiguration = null;
    }
}