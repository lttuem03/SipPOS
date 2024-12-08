using System.Text;
using System.Text.RegularExpressions;

using Npgsql;
using NpgsqlTypes;

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
    public async Task<(long id, StaffDto? dto)> InsertAsync(long storeId, StaffDto staffDto)
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
                new() { Value = staffDto.CreatedBy },
                new() { Value = staffDto.CreatedAt }
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
    /// Gets a list of staff records with pagination.
    /// </summary>
    /// <param name="storeId">The ID of the store to retrieve staff records for.</param>
    /// <param name="page">The current page of the pagination.</param>
    /// <param name="rowsPerPage">The rows per page count of the pagination.</param>
    /// <param name="keyword">The search keyword for the 'name' column.</param>
    /// <param name="sortBy">The name of the column in the database schema (writen in snake_case).</param>
    /// <param name="sortDirection">'ASC' or 'DESC'</param>
    /// <param name="filterByPositionPrefixes">A nullable list containings the prefixes of filtered position.</param>
    /// <returns>A tuple, containings a number of rows that matched the criterias (only the count), and the list of (paged) staffs retrieved.</returns>
    public async Task<(long totalRowsMatched, List<StaffDto>? staffDtos)> GetWithPagination
    (
        long storeId,
        long page,
        long rowsPerPage,
        string keyword = "",
        string? sortBy = null,
        string? sortDirection = null,
        List<string>? filterByPositionPrefixes = null
    )
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();
        using var getCountConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;
        using var getDataConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        var countQuery = new StringBuilder();  // to count total rows matched the criterias
        var pagedQuery = new StringBuilder();  // to get actual data
        var take = rowsPerPage;
        var skip = (page - 1) * rowsPerPage;

        // Because of the way Npgsql handles parameters (using proposional parameters)
        // we have to split into 2 cases (and possibly many mini-cases):
        // - The one where searching is included
        // - The one where searching is not included

        // Include search by keywords
        if (!string.IsNullOrEmpty(keyword))
        {
            // Sorting options if not provided
            sortBy ??= "position_prefix";
            sortDirection ??= "ASC";

            // DOESN'T filter by position prefixes
            if (filterByPositionPrefixes == null)
            {
                countQuery.Append(@"
                    SELECT COUNT(*) FROM staff
                    WHERE (store_id = $1)
                    AND (name LIKE $2)
                    AND (employment_status NOT LIKE '%OutOfEmployment%')
                ");

                pagedQuery.Append(@"
                    SELECT * FROM staff
                    WHERE (store_id = $1)
                    AND (name LIKE $2)
                    AND (employment_status NOT LIKE '%OutOfEmployment%')
                ");

                pagedQuery.AppendLine($"ORDER BY {sortBy} {sortDirection.ToUpper()}");
                pagedQuery.AppendLine($"LIMIT $3");
                pagedQuery.AppendLine($"OFFSET $4");

                await using var getCountCommand = new NpgsqlCommand(countQuery.ToString(), getCountConnection)
                {
                    Parameters =
                    {
                        new() { Value = storeId },
                        new() { Value = $"%{keyword}%" },
                    }
                };

                await using var getDataCommand = new NpgsqlCommand(pagedQuery.ToString(), getDataConnection)
                {
                    Parameters =
                    {
                        new() { Value = storeId },
                        new() { Value = $"%{keyword}%" },
                        new() { Value = take },
                        new() { Value = skip }
                    }
                };

                var countScalar = await getCountCommand.ExecuteScalarAsync();
                var count = countScalar == null ? 0 : (long)countScalar;

                await using var reader = await getDataCommand.ExecuteReaderAsync();
                var staffDtos = await processCommandReader(reader);

                return (count, staffDtos);
            }
            else // Has filter by position prefixes
            {
                countQuery.Append(@"
                    SELECT COUNT(*) FROM staff
                    WHERE (store_id = $1)
                    AND (name LIKE $2)
                    AND (employment_status NOT LIKE '%OutOfEmployment%')
                ");

                pagedQuery.Append(@"
                    SELECT * FROM staff
                    WHERE (store_id = $1)
                    AND (name LIKE $2)
                    AND (employment_status NOT LIKE '%OutOfEmployment%')
                ");

                switch (filterByPositionPrefixes.Count)
                {
                    case 1:
                    {
                        countQuery.AppendLine($"AND (position_prefix = $3)");

                        pagedQuery.AppendLine($"AND (position_prefix = $3)");
                        pagedQuery.AppendLine($"ORDER BY {sortBy} {sortDirection.ToUpper()}");
                        pagedQuery.AppendLine($"LIMIT $4");
                        pagedQuery.AppendLine($"OFFSET $5");

                        await using var getCountCommand = new NpgsqlCommand(countQuery.ToString(), getCountConnection)
                        {
                            Parameters =
                            {
                                new() { Value = storeId },
                                new() { Value = $"%{keyword}%" },
                                new() { Value = filterByPositionPrefixes[0]},
                            }
                        };

                        await using var getDataCommand = new NpgsqlCommand(pagedQuery.ToString(), getDataConnection)
                        {
                            Parameters =
                            {
                                new() { Value = storeId },
                                new() { Value = $"%{keyword}%" },
                                new() { Value = filterByPositionPrefixes[0]},
                                new() { Value = take },
                                new() { Value = skip }
                            }
                        };

                        var countScalar = await getCountCommand.ExecuteScalarAsync();
                        var count = countScalar == null ? 0 : (long)countScalar;

                        await using var reader = await getDataCommand.ExecuteReaderAsync();
                        var staffDtos = await processCommandReader(reader);

                        return (count, staffDtos);
                    }
                    case 2:
                    {
                        countQuery.AppendLine($"AND (position_prefix = $3 OR position_prefix = $4)");

                        pagedQuery.AppendLine($"AND (position_prefix = $3 OR position_prefix = $4)");
                        pagedQuery.AppendLine($"ORDER BY {sortBy} {sortDirection.ToUpper()}");
                        pagedQuery.AppendLine($"LIMIT $5");
                        pagedQuery.AppendLine($"OFFSET $6");

                        await using var getCountCommand = new NpgsqlCommand(countQuery.ToString(), getCountConnection)
                        {
                            Parameters =
                            {
                                new() { Value = storeId },
                                new() { Value = $"%{keyword}%" },
                                new() { Value = filterByPositionPrefixes[0]},
                                new() { Value = filterByPositionPrefixes[1]},
                            }
                        };

                        await using var getDataCommand = new NpgsqlCommand(pagedQuery.ToString(), getDataConnection)
                        {
                            Parameters =
                            {
                                new() { Value = storeId },
                                new() { Value = $"%{keyword}%" },
                                new() { Value = filterByPositionPrefixes[0]},
                                new() { Value = filterByPositionPrefixes[1]},
                                new() { Value = take },
                                new() { Value = skip }
                            }
                        };

                        var countScalar = await getCountCommand.ExecuteScalarAsync();
                        var count = countScalar == null ? 0 : (long)countScalar;

                        await using var reader = await getDataCommand.ExecuteReaderAsync();
                        var staffDtos = await processCommandReader(reader);

                        return (count, staffDtos);
                    }
                    case 3:
                    {
                        countQuery.AppendLine($"AND (position_prefix = $3 OR position_prefix = $4 OR position_prefix = $5)");

                        pagedQuery.AppendLine($"AND (position_prefix = $3 OR position_prefix = $4 OR position_prefix = $5)");
                        pagedQuery.AppendLine($"ORDER BY {sortBy} {sortDirection.ToUpper()}");
                        pagedQuery.AppendLine($"LIMIT $6");
                        pagedQuery.AppendLine($"OFFSET $7");

                        await using var getCountCommand = new NpgsqlCommand(countQuery.ToString(), getCountConnection)
                        {
                            Parameters =
                            {
                                new() { Value = storeId },
                                new() { Value = $"%{keyword}%" },
                                new() { Value = filterByPositionPrefixes[0]},
                                new() { Value = filterByPositionPrefixes[1]},
                                new() { Value = filterByPositionPrefixes[2]},
                            }
                        };

                        await using var getDataCommand = new NpgsqlCommand(pagedQuery.ToString(), getDataConnection)
                        {
                            Parameters =
                            {
                                new() { Value = storeId },
                                new() { Value = $"%{keyword}%" },
                                new() { Value = filterByPositionPrefixes[0]},
                                new() { Value = filterByPositionPrefixes[1]},
                                new() { Value = filterByPositionPrefixes[2]},
                                new() { Value = take },
                                new() { Value = skip }
                            }
                        };

                        var countScalar = await getCountCommand.ExecuteScalarAsync();
                        var count = countScalar == null ? 0 : (long)countScalar;

                        await using var reader = await getDataCommand.ExecuteReaderAsync();
                        var staffDtos = await processCommandReader(reader);

                        return (count, staffDtos);
                    }    
                }
            }
        }
        else // DO NOT nnclude search by keywords
        {
            // Sorting options if not provided
            sortBy ??= "position_prefix";
            sortDirection ??= "ASC";

            // DOESN'T filter by position prefixes
            if (filterByPositionPrefixes == null)
            {
                countQuery.Append(@"
                    SELECT COUNT(*) FROM staff 
                    WHERE (store_id = $1)
                    AND (employment_status NOT LIKE '%OutOfEmployment%')
                ");

                pagedQuery.Append(@"
                    SELECT * FROM staff
                    WHERE (store_id = $1)
                    AND (employment_status NOT LIKE '%OutOfEmployment%')
                ");

                pagedQuery.AppendLine($"ORDER BY {sortBy} {sortDirection.ToUpper()}");
                pagedQuery.AppendLine($"LIMIT $2");
                pagedQuery.AppendLine($"OFFSET $3");

                await using var getCountCommand = new NpgsqlCommand(countQuery.ToString(), getCountConnection)
                {
                    Parameters =
                    {
                        new() { Value = storeId }
                    }
                };

                await using var getDataCommand = new NpgsqlCommand(pagedQuery.ToString(), getDataConnection)
                {
                    Parameters =
                    {
                        new() { Value = storeId },
                        new() { Value = take },
                        new() { Value = skip }
                    }
                };

                var countScalar = await getCountCommand.ExecuteScalarAsync();
                var count = countScalar == null ? 0 : (long)countScalar;

                await using var reader = await getDataCommand.ExecuteReaderAsync();
                var staffDtos = await processCommandReader(reader);

                return (count, staffDtos);
            }
            else // Has filter by position prefixes
            {
                countQuery.Append(@"
                    SELECT COUNT(*) FROM staff 
                    WHERE (store_id = $1)
                    AND (employment_status NOT LIKE '%OutOfEmployment%')
                ");

                pagedQuery.Append(@"
                    SELECT * FROM staff
                    WHERE (store_id = $1)
                    AND (employment_status NOT LIKE '%OutOfEmployment%')
                ");

                switch (filterByPositionPrefixes.Count)
                {
                    case 1:
                    {
                        countQuery.AppendLine($"AND (position_prefix = $2)");

                        pagedQuery.AppendLine($"AND (position_prefix = $2)");
                        pagedQuery.AppendLine($"ORDER BY {sortBy} {sortDirection.ToUpper()}");
                        pagedQuery.AppendLine($"LIMIT $3");
                        pagedQuery.AppendLine($"OFFSET $4");

                        await using var getCountCommand = new NpgsqlCommand(countQuery.ToString(), getCountConnection)
                        {
                            Parameters =
                            {
                                new() { Value = storeId },
                                new() { Value = filterByPositionPrefixes[0]}
                            }
                        };

                        await using var getDataCommand = new NpgsqlCommand(pagedQuery.ToString(), getDataConnection)
                        {
                            Parameters =
                            {
                                new() { Value = storeId },
                                new() { Value = filterByPositionPrefixes[0]},
                                new() { Value = take },
                                new() { Value = skip }
                            }
                        };

                        var countScalar = await getCountCommand.ExecuteScalarAsync();
                        var count = countScalar == null ? 0 : (long)countScalar;

                        await using var reader = await getDataCommand.ExecuteReaderAsync();
                        var staffDtos = await processCommandReader(reader);

                        return (count, staffDtos);
                    }
                    case 2:
                    {
                        countQuery.AppendLine($"AND (position_prefix = $2 OR position_prefix = $3)");

                        pagedQuery.AppendLine($"AND (position_prefix = $2 OR position_prefix = $3)");
                        pagedQuery.AppendLine($"ORDER BY {sortBy} {sortDirection.ToUpper()}");
                        pagedQuery.AppendLine($"LIMIT $4");
                        pagedQuery.AppendLine($"OFFSET $5");

                        await using var getCountCommand = new NpgsqlCommand(countQuery.ToString(), getCountConnection)
                        {
                            Parameters =
                            {
                                new() { Value = storeId },
                                new() { Value = filterByPositionPrefixes[0]},
                                new() { Value = filterByPositionPrefixes[1]}
                            }
                        };

                        await using var getDataCommand = new NpgsqlCommand(pagedQuery.ToString(), getDataConnection)
                        {
                            Parameters =
                            {
                                new() { Value = storeId },
                                new() { Value = filterByPositionPrefixes[0]},
                                new() { Value = filterByPositionPrefixes[1]},
                                new() { Value = take },
                                new() { Value = skip }
                            }
                        };

                        var countScalar = await getCountCommand.ExecuteScalarAsync();
                        var count = countScalar == null ? 0 : (long)countScalar;

                        await using var reader = await getDataCommand.ExecuteReaderAsync();
                        var staffDtos = await processCommandReader(reader);

                        return (count, staffDtos);
                    }
                    case 3:
                    {
                        countQuery.AppendLine($"AND (position_prefix = $2 OR position_prefix = $3 OR position_prefix = $4)");

                        pagedQuery.AppendLine($"AND (position_prefix = $2 OR position_prefix = $3 OR position_prefix = $4)");
                        pagedQuery.AppendLine($"ORDER BY {sortBy} {sortDirection.ToUpper()}");
                        pagedQuery.AppendLine($"LIMIT $5");
                        pagedQuery.AppendLine($"OFFSET $6");

                        await using var getCountCommand = new NpgsqlCommand(countQuery.ToString(), getCountConnection)
                        {
                            Parameters =
                        {
                            new() { Value = storeId },
                            new() { Value = filterByPositionPrefixes[0]},
                            new() { Value = filterByPositionPrefixes[1]},
                            new() { Value = filterByPositionPrefixes[2]}
                        }
                        };

                        await using var getDataCommand = new NpgsqlCommand(pagedQuery.ToString(), getDataConnection)
                        {
                            Parameters =
                        {
                            new() { Value = storeId },
                            new() { Value = filterByPositionPrefixes[0]},
                            new() { Value = filterByPositionPrefixes[1]},
                            new() { Value = filterByPositionPrefixes[2]},
                            new() { Value = take },
                            new() { Value = skip }
                        }
                        };

                        var countScalar = await getCountCommand.ExecuteScalarAsync();
                        var count = countScalar == null ? 0 : (long)countScalar;

                        await using var reader = await getDataCommand.ExecuteReaderAsync();
                        var staffDtos = await processCommandReader(reader);

                        return (count, staffDtos);
                    }
                }
            }
        }

        return (0, null);
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

        if (updatedStaffDto.PasswordHash == string.Empty)
        {
            updatedStaffDto.PasswordHash = "*".PadLeft(64); // to sastify constraint of the database schema
        }

        if (updatedStaffDto.Salt == string.Empty)
        {
            updatedStaffDto.Salt = "*".PadLeft(64); // to sastify constraint of the database schema
        }

        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            UPDATE staff
            SET updated_by = $1,
                updated_at = $2,
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
                new() { Value = updatedStaffDto.EmploymentEndDate ?? (object)DBNull.Value, NpgsqlDbType = NpgsqlDbType.Date },
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

    private async Task<List<StaffDto>?> processCommandReader(NpgsqlDataReader? reader)
    {
        if (reader == null)
            return null;

        if (reader.HasRows)
        {
            List<StaffDto> staffDtos = new();

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
                    PasswordHash = string.Empty,    // not needed in the context of Get
                    Salt = string.Empty,            // not needed in the context of Get
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

                staffDtos.Add(newStaffDto);
            }

            return staffDtos;
        }

        return null;
    }
}