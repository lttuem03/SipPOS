using Npgsql;

using SipPOS.DataTransfer.General;
using SipPOS.Services.General.Interfaces;
using SipPOS.Services.DataAccess.Interfaces;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace SipPOS.Services.DataAccess.Implementations;

public class PostgreConfigurationDao : IConfigurationDao
{
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
                staff_base_salary,
                staff_hourly_salary,
                assistant_manager_base_salary,
                assistant_manager_hourly_salary,
                store_manager_base_salary,
                store_manager_hourly_salary
            )
            VALUES
                ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12)
            RETURNING
                store_id,
                opening_time,
                closing_time,
                tax_code,
                vat_rate,
                vat_method,
                staff_base_salary,
                staff_hourly_salary,
                assistant_manager_base_salary,
                assistant_manager_hourly_salary,
                store_manager_base_salary,
                store_manager_hourly_salary
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
                new() { Value = configurationDto.StaffBaseSalary },
                new() { Value = configurationDto.StaffHourlySalary },
                new() { Value = configurationDto.AssistantManagerBaseSalary },
                new() { Value = configurationDto.AssistantManagerHourlySalary },
                new() { Value = configurationDto.StoreManagerBaseSalary },
                new() { Value = configurationDto.StoreManagerHourlySalary }
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
                StaffBaseSalary = reader.GetDecimal(reader.GetOrdinal("staff_base_salary")),
                StaffHourlySalary = reader.GetDecimal(reader.GetOrdinal("staff_hourly_salary")),
                AssistantManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("assistant_manager_base_salary")),
                AssistantManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("assistant_manager_hourly_salary")),
                StoreManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("store_manager_base_salary")),
                StoreManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("store_manager_hourly_salary"))
            };
        }

        return null;
    }

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
                StaffBaseSalary = reader.GetDecimal(reader.GetOrdinal("staff_base_salary")),
                StaffHourlySalary = reader.GetDecimal(reader.GetOrdinal("staff_hourly_salary")),
                AssistantManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("assistant_manager_base_salary")),
                AssistantManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("assistant_manager_hourly_salary")),
                StoreManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("store_manager_base_salary")),
                StoreManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("store_manager_hourly_salary"))
            };
        }

        return null;
    }

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
                staff_base_salary = $6,
                staff_hourly_salary = $7,
                assistant_manager_base_salary = $8,
                assistant_manager_hourly_salary = $9,
                store_manager_base_salary = $10,
                store_manager_hourly_salary = $11
            WHERE store_id = $12
        ", connection)
        {
            Parameters =
            {
                new() { Value = updatedConfigurationDto.OpeningTime },
                new() { Value = updatedConfigurationDto.ClosingTime },
                new() { Value = updatedConfigurationDto.TaxCode },
                new() { Value = updatedConfigurationDto.VatRate },
                new() { Value = updatedConfigurationDto.VatMethod },
                new() { Value = updatedConfigurationDto.StaffBaseSalary },
                new() { Value = updatedConfigurationDto.StaffHourlySalary },
                new() { Value = updatedConfigurationDto.AssistantManagerBaseSalary },
                new() { Value = updatedConfigurationDto.AssistantManagerHourlySalary },
                new() { Value = updatedConfigurationDto.StoreManagerBaseSalary },
                new() { Value = updatedConfigurationDto.StoreManagerHourlySalary },
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
                StaffBaseSalary = reader.GetDecimal(reader.GetOrdinal("staff_base_salary")),
                StaffHourlySalary = reader.GetDecimal(reader.GetOrdinal("staff_hourly_salary")),
                AssistantManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("assistant_manager_base_salary")),
                AssistantManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("assistant_manager_hourly_salary")),
                StoreManagerBaseSalary = reader.GetDecimal(reader.GetOrdinal("store_manager_base_salary")),
                StoreManagerHourlySalary = reader.GetDecimal(reader.GetOrdinal("store_manager_hourly_salary"))
            };
        }

        return null;
    }
}