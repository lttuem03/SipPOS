using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Npgsql;
using SipPOS.DataTransfer.Entity;
using SipPOS.Models.Entity;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;

namespace SipPOS.Services.DataAccess.Implementations;

/// <summary>
/// Data Access Object for Store using PostgreSQL database.
/// </summary>
public class PostgreStoreDao : IStoreDao
{
    /// <summary>
    /// Inserts a new store asynchronously.
    /// </summary>
    /// <param name="storeDto">The store data transfer object containing store details.</param>
    /// <returns>The inserted store if successful; otherwise, null.</returns>
    public async Task<(long id, StoreDto? dto)> InsertAsync(StoreDto storeDto)
    {
        if (storeDto.PasswordHash == null ||
            storeDto.Salt == null ||
            storeDto.CreatedBy == null ||
            storeDto.CreatedAt == null)
        {
            return (-1, null);
        }

        // All field validations must be done before calling this method
        // In this codebase: Validations is done via StoreAccountCreationService.ValidateFields()
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            INSERT INTO store (
                name,
                address,
                email,
                tel,
                username,
                password_hash,
                salt,
                last_login,
                created_by,
                created_at
            )
            VALUES 
                ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10)
            RETURNING 
                id,
                name,
                address,
                email,
                tel,
                username,
                password_hash,
                salt,
                last_login,
                created_by,
                created_at
        ", connection)
        {
            Parameters =
            {
                new() { Value = storeDto.Name },
                new() { Value = storeDto.Address },
                new() { Value = storeDto.Email },
                new() { Value = storeDto.Tel },
                new() { Value = storeDto.Username },
                new() { Value = storeDto.PasswordHash },
                new() { Value = storeDto.Salt },
                new() { Value = storeDto.LastLogin },
                new() { Value = storeDto.CreatedBy },
                new() { Value = storeDto.CreatedAt }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                return (
                    id: reader.GetInt32(reader.GetOrdinal("id")),
                    dto: new StoreDto()
                    {
                        // BaseModel fields
                        CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        // Store Model fields
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Address = reader.GetString(reader.GetOrdinal("address")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Tel = reader.GetString(reader.GetOrdinal("tel")),
                        Username = reader.GetString(reader.GetOrdinal("username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                        Salt = reader.GetString(reader.GetOrdinal("salt")),
                        LastLogin = reader.GetDateTime(reader.GetOrdinal("last_login"))
                    }
                );
            }
        }

        return (-1, null);
    }

    /// <summary>
    /// Retrieves a store by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the store to retrieve.</param>
    /// <returns>The store data transfer object if found; otherwise, null.</returns>
    public Task<StoreDto?> GetByIdAsync(long id) => throw new NotImplementedException();

    /// <summary>
    /// Retrieves a store by its username asynchronously.
    /// </summary>
    /// <param name="username">The username of the store to retrieve.</param>
    /// <returns>The store data transfer object if found; otherwise, null.</returns>
    public async Task<StoreDto?> GetByUsernameAsync(string username)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(
            "SELECT * FROM store WHERE username = @username",
            connection
        );

        command.Parameters.AddWithValue("@username", username);

        await using var reader = await command.ExecuteReaderAsync();

        // The database schema only allow unique username, so this command
        // execution should only return 0 or 1 row
        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                StoreDto storeDto = new StoreDto()
                {
                    // BaseModel fields
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                    // Store Model fields
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Tel = reader.GetString(reader.GetOrdinal("tel")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                    Salt = reader.GetString(reader.GetOrdinal("salt")),
                    LastLogin = reader.GetDateTime(reader.GetOrdinal("last_login"))
                };

                // Handle nullable columns
                storeDto.UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by")) ? 
                    null : reader.GetString(reader.GetOrdinal("updated_by"));

                storeDto.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ?
                    null : reader.GetDateTime(reader.GetOrdinal("updated_at"));

                storeDto.DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by")) ?
                    null : reader.GetString(reader.GetOrdinal("deleted_by"));

                storeDto.DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) ?
                    null : reader.GetDateTime(reader.GetOrdinal("deleted_at"));

                return storeDto;
            }
        }

        return null;
    }

    /// <summary>
    /// Updates a store by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the store to update.</param>
    /// <param name="updatedStoreDto">The updated store data transfer object.</param>
    /// <returns>The updated Store DTO (get new info from the database) if successful; otherwise, null.</returns>
    public Task<StoreDto?> UpdateByIdAsync(long id, StoreDto updatedStoreDto) => throw new NotImplementedException();

    /// <summary>
    /// Updates a store by its username asynchronously.
    /// </summary>
    /// <param name="username">The username of the store to update.</param>
    /// <param name="updatedStoreDto">The updated store data transfer object.</param>
    /// <returns>The updated Store DTO (get new info from the database) if successful; otherwise, null.</returns>
    public async Task<StoreDto?> UpdateByUsernameAsync(string username, StoreDto updatedStoreDto)
    {
        // Please make sure the fields are properly validated before calling this method

        if (username != updatedStoreDto.Username)
        {
            return null;
        }

        if (updatedStoreDto.PasswordHash == null ||
            updatedStoreDto.Salt == null ||
            updatedStoreDto.UpdatedBy == null ||
            updatedStoreDto.UpdatedAt == null)
        {
            return null;
        }

        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            UPDATE store
            SET name = $1,
                address = $2,
                email = $3,
                tel = $4,
                password_hash = $5,
                salt = $6,
                last_login = $7,
                updated_by = $8,
                updated_at = $9
            WHERE username = $10
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = updatedStoreDto.Name },
                new() { Value = updatedStoreDto.Address },
                new() { Value = updatedStoreDto.Email },
                new() { Value = updatedStoreDto.Tel },
                new() { Value = updatedStoreDto.PasswordHash },
                new() { Value = updatedStoreDto.Salt },
                new() { Value = updatedStoreDto.LastLogin },
                new() { Value = updatedStoreDto.UpdatedBy },
                new() { Value = updatedStoreDto.UpdatedAt },
                new() { Value = username }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        // The database schema only allow unique username, so this command
        // execution should only return 0 or 1 row
        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                return new StoreDto
                {
                    // BaseModel fields
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                    UpdatedBy = reader.GetString(reader.GetOrdinal("updated_by")),
                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("updated_at")),
                    // Store Model fields
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Tel = reader.GetString(reader.GetOrdinal("tel")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                    Salt = reader.GetString(reader.GetOrdinal("salt")),
                    LastLogin = reader.GetDateTime(reader.GetOrdinal("last_login"))
                };
            }
        }

        return null;
    }

    /// <summary>
    /// Deletes a store by its ID asynchronously (perform a soft delete in database).
    /// </summary>
    /// <param name="id">The ID of the store to delete.</param>
    /// <returns>The deleted store data transfer object if successful; otherwise, null.</returns>
    public Task<StoreDto?> DeleteByIdAsync(long id) => throw new NotImplementedException();

    /// <summary>
    /// Deletes a store by its username asynchronously (perform a soft delete in database).
    /// </summary>
    /// <param name="username">The username of the store to delete.</param>
    /// <returns>The deleted store data transfer object if successful; otherwise, null.</returns>
    public Task<StoreDto?> DeleteByUsernameAsync(string username) => throw new NotImplementedException();
}
