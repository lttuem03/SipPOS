using System.Threading.Tasks;

using SipPOS.DataTransfer.General;

namespace SipPOS.Services.DataAccess.Interfaces;

public interface IConfigurationDao
{
    Task<ConfigurationDto?> InsertAsync(long storeId, ConfigurationDto configurationDto);
    Task<ConfigurationDto?> GetByIdAsync(long storeId);
    Task<ConfigurationDto?> UpdateByIdAsync(long storeId, ConfigurationDto updatedConfigurationDto);
}