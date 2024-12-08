using System.Text.RegularExpressions;

using Npgsql;

using SipPOS.DataTransfer.Entity;
using SipPOS.Models.Entity;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;

namespace SipPOS.Services.DataAccess.Implementations;

/// <summary>
/// Data Access Object for Staff using PostgreSQL database.
/// </summary>
public class PostgreStaffDao : IStaffDao
{
    /// <summary>
    /// Inserts a new staff record into the database.
    /// </summary>
    /// <param name="storeId">The ID of the store to which the staff belongs.</param>
    /// <param name="staffDto">The data transfer object containing staff details.</param>
    /// <param name="author">The staff member who is creating the new record.</param>
    /// <returns>A tuple containing the ID of the newly created staff record and the corresponding StaffDto object.</returns>
    /// <remarks>
    /// All field validations must be done before calling this method. In this codebase, validations are done via StaffAccountCreationService.ValidateFields().
    /// </remarks>
    public async Task<(long id, StaffDto? dto)> InsertAsync(long storeId, StaffDto staffDto, Staff author)
    {
        if (staffDto.PasswordHash == null ||
            staffDto.Salt == null ||
            staffDto.CreatedBy == null ||
            staffDto.CreatedAt == null)
        {
            return (-1, null);
        }

        // All field validations must be done before calling this method
        // In this codebase: Validations is done via StaffAccountCreationService.ValidateFields()
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            INSERT INTO staff (
                store_id, 
                position_prefix, 
                password_hash, 
                salt, 
                name, 
                gender,
                date_of_birth, 
                email, 
                tel, 
                address, 
                employment_status, 
                employment_start_date, 
                created_by, 
                created_at
            )
            VALUES 
                ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14)
            RETURNING 
                id,
                store_id, 
                position_prefix, 
                password_hash, 
                salt, 
                name, 
                gender,
                date_of_birth, 
                email, 
                tel, 
                address, 
                employment_status, 
                employment_start_date, 
                created_by, 
                created_at
        ", connection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = staffDto.PositionPrefix },
                new() { Value = staffDto.PasswordHash },
                new() { Value = staffDto.Salt },
                new() { Value = staffDto.Name },
                new() { Value = staffDto.Gender },
                new() { Value = staffDto.DateOfBirth },
                new() { Value = staffDto.Email },
                new() { Value = staffDto.Tel },
                new() { Value = staffDto.Address },
                new() { Value = staffDto.EmploymentStatus },
                new() { Value = staffDto.EmploymentStartDate },
                new() { Value = author.CompositeUsername },
                new() { Value = DateTime.Now }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                return (
                    id: reader.GetInt64(reader.GetOrdinal("id")),
                    dto: new StaffDto()
                    {
                        // BaseModel fields
                        CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        // Staff fields
                        StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
                        PositionPrefix = reader.GetString(reader.GetOrdinal("position_prefix")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                        Salt = reader.GetString(reader.GetOrdinal("salt")),
                        Name = reader.GetString(reader.GetOrdinal("name")),
                        Gender = reader.GetString(reader.GetOrdinal("gender")),
                        DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Tel = reader.GetString(reader.GetOrdinal("tel")),
                        Address = reader.GetString(reader.GetOrdinal("address")),
                        EmploymentStatus = reader.GetString(reader.GetOrdinal("employment_status")),
                        EmploymentStartDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_start_date")))
                    }
                );
            }
        }

        return (-1, null);
    }

    /// <summary>
    /// Retrieves all staff records for a given store.
    /// </summary>
    /// <param name="storeId">The ID of the store to retrieve staff records for.</param>
    /// <returns>A list of StaffDto objects representing the staff records, or null if no records are found.</returns>
    public async Task<List<StaffDto>?> GetAllAsync(long storeId)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            SELECT * FROM staff WHERE store_id = $1
        ", connection)
        {
            Parameters = { new() { Value = storeId } }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            List<StaffDto> staffDtoList = new();

            while (await reader.ReadAsync())
            {
                StaffDto newStaffDto = new StaffDto()
                {
                    // BaseModel fields
                    Id = reader.GetInt64(reader.GetOrdinal("id")),
                    CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),   // ensured not null in creation
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")), // ensured not null in creation
                    // Staff fields
                    StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
                    PositionPrefix = reader.GetString(reader.GetOrdinal("position_prefix")),
                    PasswordHash = string.Empty,    // not needed in the context of GetAll
                    Salt = string.Empty,            // not needed in the context of GetAll
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Gender = reader.GetString(reader.GetOrdinal("gender")),
                    DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Tel = reader.GetString(reader.GetOrdinal("tel")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    EmploymentStatus = reader.GetString(reader.GetOrdinal("employment_status")),
                    EmploymentStartDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_start_date")))
                };

                // Handle nullable columns
                newStaffDto.UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by")) == true ?
                    null : reader.GetString(reader.GetOrdinal("updated_by"));

                newStaffDto.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) == true ?
                    null : reader.GetDateTime(reader.GetOrdinal("updated_at"));

                newStaffDto.DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by")) == true ?
                    null : reader.GetString(reader.GetOrdinal("deleted_by"));

                newStaffDto.DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) == true ?
                    null : reader.GetDateTime(reader.GetOrdinal("deleted_at"));

                newStaffDto.EmploymentEndDate = reader.IsDBNull(reader.GetOrdinal("employment_end_date")) == true ?
                    null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_end_date")));

                staffDtoList.Add(newStaffDto);
            }

            return staffDtoList;
        }

        return null;
    }

    /// <summary>
    /// Retrieves a staff record by its ID and store ID.
    /// </summary>
    /// <param name="storeId">The ID of the store to which the staff belongs.</param>
    /// <param name="id">The ID of the staff to retrieve.</param>
    /// <returns>A StaffDto object representing the staff record, or null if no record is found.</returns>
    public async Task<StaffDto?> GetByIdAsync(long storeId, long id)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            SELECT * FROM staff 
            WHERE store_id = $1 AND id = $2
        ", connection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = id }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                StaffDto staffDto = new StaffDto()
                {
                    // BaseModel fields
                    Id = reader.GetInt64(reader.GetOrdinal("id")),
                    CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),   // ensured not null in creation
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")), // ensured not null in creation
                    // Staff fields
                    StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
                    PositionPrefix = reader.GetString(reader.GetOrdinal("position_prefix")),
                    PasswordHash = string.Empty,    // not needed in the context of GetById
                    Salt = string.Empty,            // not needed in the context of GetById
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Gender = reader.GetString(reader.GetOrdinal("gender")),
                    DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Tel = reader.GetString(reader.GetOrdinal("tel")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    EmploymentStatus = reader.GetString(reader.GetOrdinal("employment_status")),
                    EmploymentStartDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_start_date")))
                };

                // Handle nullable columns
                staffDto.UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by")) == true ?
                    null : reader.GetString(reader.GetOrdinal("updated_by"));

                staffDto.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) == true ?
                    null : reader.GetDateTime(reader.GetOrdinal("updated_at"));

                staffDto.DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by")) == true ?
                    null : reader.GetString(reader.GetOrdinal("deleted_by"));

                staffDto.DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) == true ?
                    null : reader.GetDateTime(reader.GetOrdinal("deleted_at"));

                staffDto.EmploymentEndDate = reader.IsDBNull(reader.GetOrdinal("employment_end_date")) == true ?
                    null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_end_date")));

                return staffDto;
            }
        }

        return null;
    }

    /// <summary>
    /// Retrieves a staff record by its composite username.
    /// </summary>
    /// <param name="compositeUsername">The composite username of the staff to retrieve.</param>
    /// <returns>A StaffDto object representing the staff record, or null if no record is found or the composite username is invalid.</returns>
    public async Task<StaffDto?> GetByCompositeUsername(string compositeUsername)
    {
        // Splits composite username into (position prefix, store id, staff id)
        Regex re = new Regex(Staff.CompositeUsernamePattern);
        Match match = re.Match(compositeUsername);

        if (!match.Success)
        {
            return null;
        }

        var positionPrefix = match.Groups[1].Value;
        long storeId;
        long staffId;

        try
        {
            storeId = Int64.Parse(match.Groups[2].Value);
            staffId = Int64.Parse(match.Groups[3].Value);
        }
        catch (FormatException)
        {
            return null;
        }

        // Fetch staff details from database
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            SELECT * FROM staff 
            WHERE position_prefix = $1 AND store_id = $2 AND id = $3
        ", connection)
        {
            Parameters =
            {
                new() { Value = positionPrefix },
                new() { Value = storeId },
                new() { Value = staffId }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                StaffDto staffDto = new StaffDto()
                {
                    // BaseEntityDto fields
                    Id = reader.GetInt64(reader.GetOrdinal("id")),
                    CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                    // Staff fields
                    StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
                    PositionPrefix = reader.GetString(reader.GetOrdinal("position_prefix")),
                    PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                    Salt = reader.GetString(reader.GetOrdinal("salt")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Gender = reader.GetString(reader.GetOrdinal("gender")),
                    DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Tel = reader.GetString(reader.GetOrdinal("tel")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    EmploymentStatus = reader.GetString(reader.GetOrdinal("employment_status")),
                    EmploymentStartDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_start_date")))
                };

                // Handle nullable columns
                staffDto.UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by")) == true ?
                    null : reader.GetString(reader.GetOrdinal("updated_by"));

                staffDto.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) == true ?
                    null : reader.GetDateTime(reader.GetOrdinal("updated_at"));

                staffDto.DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by")) == true ?
                    null : reader.GetString(reader.GetOrdinal("deleted_by"));

                staffDto.DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) == true ?
                    null : reader.GetDateTime(reader.GetOrdinal("deleted_at"));

                staffDto.EmploymentEndDate = reader.IsDBNull(reader.GetOrdinal("employment_end_date")) == true ?
                    null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_end_date")));

                return staffDto;
            }
        }

        return null;
    }

    /// <summary>
    /// Updates a staff record by its ID and store ID.
    /// </summary>
    /// <param name="storeId">The ID of the store to which the staff belongs.</param>
    /// <param name="id">The ID of the staff to update.</param>
    /// <param name="updatedStaffDto">The updated staff data transfer object.</param>
    /// <param name="author">The staff member performing the update.</param>
    /// <returns>A StaffDto object representing the updated staff record, or null if the update fails.</returns>
    public async Task<StaffDto?> UpdateByIdAsync(long storeId, long id, StaffDto updatedStaffDto, Staff author)
    {
        // Please make sure the fields are properly validated before calling this method

        if (storeId != updatedStaffDto.StoreId)
        {
            return null;
        }

        if (id != updatedStaffDto.Id)
        {
            return null;
        }

        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            UPDATE staff
            SET update_by = $1,
                update_at = $2,
                position_prefix = $3,
                password_hash = $4,
                salt = $5,
                name = $6,
                gender = $7,
                date_of_birth = $8,
                email = $9,
                tel = $10,
                address = $11,
                employment_status = $12,
                employment_start_date = $13,
                employment_end_date = $14
            WHERE store_id = $15 AND id = $16
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = author.CompositeUsername },
                new() { Value = DateTime.Now },
                new() { Value = updatedStaffDto.PositionPrefix },
                new() { Value = updatedStaffDto.PasswordHash },
                new() { Value = updatedStaffDto.Salt },
                new() { Value = updatedStaffDto.Name },
                new() { Value = updatedStaffDto.Gender },
                new() { Value = updatedStaffDto.DateOfBirth },
                new() { Value = updatedStaffDto.Email },
                new() { Value = updatedStaffDto.Tel },
                new() { Value = updatedStaffDto.Address },
                new() { Value = updatedStaffDto.EmploymentStatus },
                new() { Value = updatedStaffDto.EmploymentStartDate },
                new() { Value = updatedStaffDto.EmploymentEndDate },
                new() { Value = storeId },
                new() { Value = id }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        // The database schema only allow unique pair of (store_id, id), so this command
        // execution should only return 0 or 1 row
        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                StaffDto staffDto = new StaffDto()
                {
                    // BaseModel fields
                    Id = reader.GetInt64(reader.GetOrdinal("id")),
                    CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),   // ensured not null in creation
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")), // ensured not null in creation
                    // Staff fields
                    StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
                    PositionPrefix = reader.GetString(reader.GetOrdinal("position_prefix")),
                    PasswordHash = string.Empty,    // not needed in the context of Update (meaning you should NOT Update using the same Dto manytimes over)
                    Salt = string.Empty,            // not needed in the context of Update 
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Gender = reader.GetString(reader.GetOrdinal("gender")),
                    DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Tel = reader.GetString(reader.GetOrdinal("tel")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    EmploymentStatus = reader.GetString(reader.GetOrdinal("employment_status")),
                    EmploymentStartDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_start_date")))
                };

                // Handle nullable columns
                staffDto.UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by")) == true ?
                    null : reader.GetString(reader.GetOrdinal("updated_by"));

                staffDto.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) == true ?
                    null : reader.GetDateTime(reader.GetOrdinal("updated_at"));

                staffDto.DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by")) == true ?
                    null : reader.GetString(reader.GetOrdinal("deleted_by"));

                staffDto.DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) == true ?
                    null : reader.GetDateTime(reader.GetOrdinal("deleted_at"));

                staffDto.EmploymentEndDate = reader.IsDBNull(reader.GetOrdinal("employment_end_date")) == true ?
                    null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_end_date")));

                return staffDto;
            }
        }

        return null;
    }

    /// <summary>
    /// Marks a staff record as deleted by its ID and store ID.
    /// </summary>
    /// <param name="storeId">The ID of the store to which the staff belongs.</param>
    /// <param name="id">The ID of the staff to delete.</param>
    /// <param name="author">The staff member performing the deletion.</param>
    /// <returns>A StaffDto object representing the deleted staff record, or null if the deletion fails.</returns>
    public async Task<StaffDto?> DeleteByIdAsync(long storeId, long id, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            UPDATE staff
            SET deleted_by = $1,
                deleted_at = $2,
            WHERE store_id = $3 AND id = $4
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = author.CompositeUsername },
                new() { Value = DateTime.Now },
                new() { Value = storeId },
                new() { Value = id }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        // The database schema only allow unique pair of (store_id, id), so this command
        // execution should only return 0 or 1 row
        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                StaffDto staffDto = new StaffDto()
                {
                    // BaseModel fields
                    Id = reader.GetInt64(reader.GetOrdinal("id")),
                    CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),   // ensured not null in creation
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")), // ensured not null in creation
                    // Staff fields
                    StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
                    PositionPrefix = reader.GetString(reader.GetOrdinal("position_prefix")),
                    PasswordHash = string.Empty,    // not needed in the context of Delete
                    Salt = string.Empty,            // not needed in the context of Delete 
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Gender = reader.GetString(reader.GetOrdinal("gender")),
                    DateOfBirth = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("date_of_birth"))),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Tel = reader.GetString(reader.GetOrdinal("tel")),
                    Address = reader.GetString(reader.GetOrdinal("address")),
                    EmploymentStatus = reader.GetString(reader.GetOrdinal("employment_status")),
                    EmploymentStartDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_start_date")))
                };

                // Handle nullable columns
                staffDto.UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by")) == true ?
                    null : reader.GetString(reader.GetOrdinal("updated_by"));

                staffDto.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) == true ?
                    null : reader.GetDateTime(reader.GetOrdinal("updated_at"));

                staffDto.DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by")) == true ?
                    null : reader.GetString(reader.GetOrdinal("deleted_by"));

                staffDto.DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) == true ?
                    null : reader.GetDateTime(reader.GetOrdinal("deleted_at"));

                staffDto.EmploymentEndDate = reader.IsDBNull(reader.GetOrdinal("employment_end_date")) == true ?
                    null : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("employment_end_date")));

                return staffDto;
            }
        }

        return null;
    }
}