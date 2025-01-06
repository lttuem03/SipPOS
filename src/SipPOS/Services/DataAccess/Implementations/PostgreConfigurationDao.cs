using SipPOS.DataTransfer.General;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;

using Npgsql;

namespace SipPOS.Services.DataAccess.Implementations;

/// <summary>
/// Data access object for configuration settings using PostgreSQL.
/// </summary>
public class PostgreConfigurationDao : IConfigurationDao
{
    /// <summary>
    /// Inserts a new configuration into the database.
    /// </summary>
    /// <param name="storeId">The store ID.</param>
    /// <param name="configurationDto">The configuration data transfer object.</param>
    /// <returns>The inserted configuration data transfer object, or <c>null</c> if the insertion failed.</returns>
    public async Task<ConfigurationDto?> InsertAsync(long storeId, ConfigurationDto configurationDto)
    {
        if (storeId < 0)
            return null;

        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            INSERT INTO configuration (
                store_id,
                opening_time,
                closing_time,
                tax_code,
                vat_rate,
                vat_method,
                current_staff_base_salary,
                current_staff_hourly_salary,
                current_assistant_manager_base_salary,
                current_assistant_manager_hourly_salary,
                current_store_manager_base_salary,
                current_store_manager_hourly_salary,
                next_staff_base_salary,
                next_staff_hourly_salary,
                next_assistant_manager_base_salary,
                next_assistant_manager_hourly_salary,
                next_store_manager_base_salary,
                next_store_manager_hourly_salary
            )
            VALUES
                ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14, $15, $16, $17, $18)
            RETURNING *
        ", connection)
        {
            Parameters = 
            {
                new() { Value = storeId },
                new() { Value = configurationDto.OpeningTime },
                new() { Value = configurationDto.ClosingTime },
                new() { Value = configurationDto.TaxCode },
                new() { Value = configurationDto.VatRate },
                new() { Value = configurationDto.VatMethod },
                new() { Value = configurationDto.CurrentStaffBaseSalary },
                new() { Value = configurationDto.CurrentStaffHourlySalary },
                new() { Value = configurationDto.CurrentAssistantManagerBaseSalary },
                new() { Value = configurationDto.CurrentAssistantManagerHourlySalary },
                new() { Value = configurationDto.CurrentStoreManagerBaseSalary },
                new() { Value = configurationDto.CurrentStoreManagerHourlySalary },
                new() { Value = configurationDto.NextStaffBaseSalary },
                new() { Value = configurationDto.NextStaffHourlySalary },
                new() { Value = configurationDto.NextAssistantManagerBaseSalary },
                new() { Value = configurationDto.NextAssistantManagerHourlySalary },
                new() { Value = configurationDto.NextStoreManagerBaseSalary },
                new() { Value = configurationDto.NextStoreManagerHourlySalary },
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
            return null;

        if (await reader.ReadAsync())
        {
            return new ConfigurationDto()
            {
                StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
                OpeningTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("opening_time"))),
                ClosingTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("closing_time"))),
                TaxCode = reader.GetString(reader.GetOrdinal("tax_code")),
                VatRate = reader.GetDecimal(reader.GetOrdinal("vat_rate")),
                VatMethod = reader.GetString(reader.GetOrdinal("vat_method")),
                CurrentStaffBaseSalary = reader.GetDecimal(reader.GetOrdinal("current_staff_base_salary")),
                CurrentStaffHourlySalary = reader.GetDecimal(reader.GetOrdinal("current_staff_hourly_salary")),
                CurrentAssistantManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("current_assistant_manager_base_salary")),
                CurrentAssistantManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("current_assistant_manager_hourly_salary")),
                CurrentStoreManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("current_store_manager_base_salary")),
                CurrentStoreManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("current_store_manager_hourly_salary")),
                NextStaffBaseSalary = reader.GetDecimal(reader.GetOrdinal("next_staff_base_salary")),
                NextStaffHourlySalary = reader.GetDecimal(reader.GetOrdinal("next_staff_hourly_salary")),
                NextAssistantManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("next_assistant_manager_base_salary")),
                NextAssistantManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("next_assistant_manager_hourly_salary")),
                NextStoreManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("next_store_manager_base_salary")),
                NextStoreManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("next_store_manager_hourly_salary"))
            };
        }

        return null;
    }

    /// <summary>
    /// Retrieves a configuration by store ID.
    /// </summary>
    /// <param name="storeId">The store ID.</param>
    /// <returns>The configuration data transfer object, or <c>null</c> if not found.</returns>
    public async Task<ConfigurationDto?> GetByIdAsync(long storeId)
    {
        if (storeId < 0)
            return null;

        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            SELECT * FROM configuration
            WHERE store_id = $1
        ", connection)
        {
            Parameters =
            {
                new() { Value = storeId }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
            return null;

        if (await reader.ReadAsync())
        {
            return new ConfigurationDto()
            {
                StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
                OpeningTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("opening_time"))),
                ClosingTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("closing_time"))),
                TaxCode = reader.GetString(reader.GetOrdinal("tax_code")),
                VatRate = reader.GetDecimal(reader.GetOrdinal("vat_rate")),
                VatMethod = reader.GetString(reader.GetOrdinal("vat_method")),
                CurrentStaffBaseSalary = reader.GetDecimal(reader.GetOrdinal("current_staff_base_salary")),
                CurrentStaffHourlySalary = reader.GetDecimal(reader.GetOrdinal("current_staff_hourly_salary")),
                CurrentAssistantManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("current_assistant_manager_base_salary")),
                CurrentAssistantManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("current_assistant_manager_hourly_salary")),
                CurrentStoreManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("current_store_manager_base_salary")),
                CurrentStoreManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("current_store_manager_hourly_salary")),
                NextStaffBaseSalary = reader.GetDecimal(reader.GetOrdinal("next_staff_base_salary")),
                NextStaffHourlySalary = reader.GetDecimal(reader.GetOrdinal("next_staff_hourly_salary")),
                NextAssistantManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("next_assistant_manager_base_salary")),
                NextAssistantManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("next_assistant_manager_hourly_salary")),
                NextStoreManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("next_store_manager_base_salary")),
                NextStoreManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("next_store_manager_hourly_salary"))
            };
        }

        return null;
    }

    /// <summary>
    /// Updates a configuration by store ID.
    /// </summary>
    /// <param name="storeId">The store ID.</param>
    /// <param name="updatedConfigurationDto">The updated configuration data transfer object.</param>
    /// <returns>The updated configuration data transfer object, or <c>null</c> if the update failed.</returns>
    public async Task<ConfigurationDto?> UpdateByIdAsync(long storeId, ConfigurationDto updatedConfigurationDto)
    {
        if (storeId < 0)
            return null;

        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            UPDATE configuration
            SET
                opening_time = $1,
                closing_time = $2,
                tax_code = $3,
                vat_rate = $4,
                vat_method = $5,
                current_staff_base_salary = $6,
                current_staff_hourly_salary = $7,
                current_assistant_manager_base_salary = $8,
                current_assistant_manager_hourly_salary = $9,
                current_store_manager_base_salary = $10,
                current_store_manager_hourly_salary = $11,
                next_staff_base_salary = $12,
                next_staff_hourly_salary = $13,
                next_assistant_manager_base_salary = $14,
                next_assistant_manager_hourly_salary = $15,
                next_store_manager_base_salary = $16,
                next_store_manager_hourly_salary = $17
            WHERE store_id = $18
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = updatedConfigurationDto.OpeningTime },
                new() { Value = updatedConfigurationDto.ClosingTime },
                new() { Value = updatedConfigurationDto.TaxCode },
                new() { Value = updatedConfigurationDto.VatRate },
                new() { Value = updatedConfigurationDto.VatMethod },
                new() { Value = updatedConfigurationDto.CurrentStaffBaseSalary },
                new() { Value = updatedConfigurationDto.CurrentStaffHourlySalary },
                new() { Value = updatedConfigurationDto.CurrentAssistantManagerBaseSalary },
                new() { Value = updatedConfigurationDto.CurrentAssistantManagerHourlySalary },
                new() { Value = updatedConfigurationDto.CurrentStoreManagerBaseSalary },
                new() { Value = updatedConfigurationDto.CurrentStoreManagerHourlySalary },
                new() { Value = updatedConfigurationDto.NextStaffBaseSalary },
                new() { Value = updatedConfigurationDto.NextStaffHourlySalary },
                new() { Value = updatedConfigurationDto.NextAssistantManagerBaseSalary },
                new() { Value = updatedConfigurationDto.NextAssistantManagerHourlySalary },
                new() { Value = updatedConfigurationDto.NextStoreManagerBaseSalary },
                new() { Value = updatedConfigurationDto.NextStoreManagerHourlySalary },
                new() { Value = storeId }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (!reader.HasRows)
            return null;

        if (await reader.ReadAsync())
        {
            return new ConfigurationDto()
            {
                StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
                OpeningTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("opening_time"))),
                ClosingTime = TimeOnly.FromTimeSpan(reader.GetTimeSpan(reader.GetOrdinal("closing_time"))),
                TaxCode = reader.GetString(reader.GetOrdinal("tax_code")),
                VatRate = reader.GetDecimal(reader.GetOrdinal("vat_rate")),
                VatMethod = reader.GetString(reader.GetOrdinal("vat_method")),
                CurrentStaffBaseSalary = reader.GetDecimal(reader.GetOrdinal("current_staff_base_salary")),
                CurrentStaffHourlySalary = reader.GetDecimal(reader.GetOrdinal("current_staff_hourly_salary")),
                CurrentAssistantManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("current_assistant_manager_base_salary")),
                CurrentAssistantManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("current_assistant_manager_hourly_salary")),
                CurrentStoreManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("current_store_manager_base_salary")),
                CurrentStoreManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("current_store_manager_hourly_salary")),
                NextStaffBaseSalary = reader.GetDecimal(reader.GetOrdinal("next_staff_base_salary")),
                NextStaffHourlySalary = reader.GetDecimal(reader.GetOrdinal("next_staff_hourly_salary")),
                NextAssistantManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("next_assistant_manager_base_salary")),
                NextAssistantManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("next_assistant_manager_hourly_salary")),
                NextStoreManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("next_store_manager_base_salary")),
                NextStoreManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("next_store_manager_hourly_salary"))
            };
        }

        return null;
    }
}