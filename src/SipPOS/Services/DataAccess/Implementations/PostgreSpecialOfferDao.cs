﻿using System.Text;

using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.DataTransfer.General;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;

using Npgsql;

namespace SipPOS.Services.DataAccess.Implementations;

/// <summary>
/// Data access object for special offers using PostgreSQL.
/// </summary>
public class PostgreSpecialOfferDao : ISpecialOfferDao
{
    /// <summary>
    /// Inserts a new special offer into the database.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="specialOffersModel">The special offer model to insert.</param>
    /// <returns>The inserted special offer, or null if the insertion failed.</returns>
    public async Task<SpecialOffer?> InsertAsync(long storeId, SpecialOffer specialOffersModel)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var specialOfferInsertConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var specialOfferInsertCommand = new NpgsqlCommand(@"
            INSERT INTO special_offer 
            (
                store_id, 
                created_by, 
                created_at, 
                name, 
                description, 
                category_id, 
                status,
                type,
                items_sold,
                max_items,
                product_id,
                start_date,
                end_date,
                discount_price,
                discount_percentage,
                code,
                price_type
            )
            VALUES 
                ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13, $14, $15, $16, $17)
            RETURNING *
        ", specialOfferInsertConnection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = specialOffersModel.CreatedBy },
                new() { Value = specialOffersModel.CreatedAt },
                new() { Value = specialOffersModel.Name },
                new() { Value = specialOffersModel.Description },
                new() { Value = specialOffersModel.CategoryId == null ? DBNull.Value : specialOffersModel.CategoryId},
                new() { Value = specialOffersModel.Status },
                new() { Value = specialOffersModel.Type },
                new() { Value = specialOffersModel.ItemsSold },
                new() { Value = specialOffersModel.MaxItems },
                new() { Value = specialOffersModel.ProductId == null ? DBNull.Value : specialOffersModel.ProductId},
                new() { Value = specialOffersModel.StartDate },
                new() { Value = specialOffersModel.EndDate },
                new() { Value = specialOffersModel.DiscountPrice },
                new() { Value = specialOffersModel.DiscountPercentage },
                new() { Value = specialOffersModel.Code },
                new() { Value = specialOffersModel.PriceType }
            }
        };

        await using var specialOfferReader = specialOfferInsertCommand.ExecuteReader();

        if (!specialOfferReader.HasRows)
            return null;

        await specialOfferReader.ReadAsync();

        return specialOfferReaderToSpecialOffer(specialOfferReader);
    }

    /// <summary>
    /// Retrieves all special offers for a given store.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <returns>A list of special offers.</returns>
    public async Task<List<SpecialOffer>> GetAllAsync(long storeId)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        // SELECT specialOffer
        using var specialOfferSelectConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ??
            throw new InvalidOperationException("Failed to open database connection.");

        await using var specialOfferSelectCommand = new NpgsqlCommand(@"
            SELECT *
            FROM special_offer
            WHERE 
                deleted_at IS NULL 
            AND 
                store_id = $1
        ", specialOfferSelectConnection)
        {
            Parameters =
            {
                new() { Value = storeId }
            }
        };

        await using var specialOfferReader = specialOfferSelectCommand.ExecuteReader();

        if (specialOfferReader.HasRows)
        {
            var specialOffers = new List<SpecialOffer>();

            while (await specialOfferReader.ReadAsync())
            {
                var specialOffer = specialOfferReaderToSpecialOffer(specialOfferReader);
                specialOffers.Add(specialOffer);
            }

            return specialOffers;
        }

        return new();
    }

    /// <summary>
    /// Retrieves a special offer by its ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="id">The ID of the special offer.</param>
    /// <returns>The special offer, or null if not found.</returns>
    public async Task<SpecialOffer?> GetByIdAsync(long storeId, long id)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>() ?? throw new InvalidOperationException("Failed to open database connection.");

        using var specialOfferSelectConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var specialOfferSelectCommand = new NpgsqlCommand(@"
            SELECT *
            FROM special_offer
            WHERE store_id = $1 AND id = $2
        ", specialOfferSelectConnection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = id }
            }
        };

        await using var specialOfferReader = specialOfferSelectCommand.ExecuteReader();

        if (specialOfferReader.HasRows)
        {
            if (await specialOfferReader.ReadAsync())
            {
                var specialOffer = specialOfferReaderToSpecialOffer(specialOfferReader);
                return specialOffer;
            }
        }

        return null;
    }

    /// <summary>
    /// Retrieves special offers with pagination and filtering.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="specialOffersFilterDto">The filter criteria for special offers.</param>
    /// <param name="sortDto">The sorting criteria.</param>
    /// <param name="page">The page number.</param>
    /// <param name="perPage">The number of items per page.</param>
    /// <returns>A pagination object containing the special offers.</returns>
    public async Task<Pagination<SpecialOffer>> GetWithPaginationAsync
    (
        long storeId,
        SpecialOfferFilterDto specialOffersFilterDto,
        SortDto sortDto,
        int page,
        int perPage
    )
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var specialOfferSelectConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ??
            throw new InvalidOperationException("Failed to open database connection.");

        var query = new StringBuilder(@"
            SELECT * 
            FROM special_offer 
            WHERE 
                deleted_at IS NULL
            AND
                store_id = $1
        ");

        var parameters = new List<NpgsqlParameter>();
        parameters.Add(new() { Value = storeId });

        var index = 2;
        if (!string.IsNullOrEmpty(specialOffersFilterDto.Code))
        {
            query.Append(" AND code ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = specialOffersFilterDto.Code });
            index++;
        }

        if (!string.IsNullOrEmpty(specialOffersFilterDto.Name))
        {
            query.Append(" AND name ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = specialOffersFilterDto.Name });
            index++;
        }

        if (!string.IsNullOrEmpty(specialOffersFilterDto.Desc))
        {
            query.Append(" AND desc ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = specialOffersFilterDto.Desc });
            index++;
        }

        if (specialOffersFilterDto.CategoryId.HasValue)
        {
            query.Append(" AND category_id = $" + index);
            parameters.Add(new() { Value = specialOffersFilterDto.CategoryId });
            index++;
        }

        if (specialOffersFilterDto.ProductId.HasValue)
        {
            query.Append(" AND product_id = $" + index);
            parameters.Add(new() { Value = specialOffersFilterDto.ProductId });
            index++;
        }

        if (!string.IsNullOrEmpty(specialOffersFilterDto.Status))
        {
            query.Append(" AND status = $" + index);
            parameters.Add(new() { Value = specialOffersFilterDto.Status });
            index++;
        }

        query.Append($" ORDER BY {sortDto.SortBy} {sortDto.SortType} LIMIT $" + index + " OFFSET $" + (index + 1));
        parameters.Add(new() { Value = perPage });
        parameters.Add(new() { Value = (page - 1) * perPage < 0 ? 0 : (page - 1) * perPage });

        await using var command = new NpgsqlCommand(query.ToString(), specialOfferSelectConnection);
        command.Parameters.AddRange(parameters.ToArray());

        await using var specialOfferReader = command.ExecuteReader();

        var totalRecord = await CountAsync(storeId, specialOffersFilterDto);

        if (specialOfferReader.HasRows)
        {
            var specialOffers = new List<SpecialOffer>();

            while (await specialOfferReader.ReadAsync())
            {
                var specialOffer = specialOfferReaderToSpecialOffer(specialOfferReader);
                specialOffers.Add(specialOffer);
            }

            return new Pagination<SpecialOffer>
            {
                Data = specialOffers,
                Page = page,
                PerPage = perPage,
                TotalRecord = totalRecord,
                TotalPage = (int)Math.Ceiling((double)totalRecord / perPage)
            };
        }

        return new Pagination<SpecialOffer>
        {
            Data = new List<SpecialOffer>(),
            Page = page,
            PerPage = perPage,
            TotalRecord = 0,
            TotalPage = 0
        };
    }

    /// <summary>
    /// Updates a special offer by its ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="specialOffersModel">The special offer model to update.</param>
    /// <returns>The updated special offer, or null if the update failed.</returns>
    public async Task<SpecialOffer?> UpdateByIdAsync(long storeId, SpecialOffer specialOffersModel)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var specialOfferUpdateConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ??
            throw new InvalidOperationException("Failed to open database connection.");

        await using var specialOfferUpdateCommand = new NpgsqlCommand(@"
            UPDATE special_offer
            SET 
                updated_by = $1,
                updated_at = $2,
                name = $3,
                description = $4,
                items_sold = $5, 
                category_id = $6,
                status = $7,
                type = $8,
                product_id = $9,
                start_date = $10,
                end_date = $11,
                discount_price = $12,
                discount_percentage = $13,
                code = $14,
                price_type = $15
            WHERE 
                store_id = $16
            AND
                id = $17
            RETURNING *
        ", specialOfferUpdateConnection)
        {
            Parameters =
            {
                new() { Value = specialOffersModel.UpdatedBy },
                new() { Value = specialOffersModel.UpdatedAt },
                new() { Value = specialOffersModel.Name },
                new() { Value = specialOffersModel.Description },
                new() { Value = specialOffersModel.ItemsSold },
                new() { Value = specialOffersModel.CategoryId == null ? DBNull.Value : specialOffersModel.CategoryId},
                new() { Value = specialOffersModel.Status },
                new() { Value = specialOffersModel.Type },
                new() { Value = specialOffersModel.ProductId == null ? DBNull.Value : specialOffersModel.ProductId},
                new() { Value = specialOffersModel.StartDate },
                new() { Value = specialOffersModel.EndDate },
                new() { Value = specialOffersModel.DiscountPrice },
                new() { Value = specialOffersModel.DiscountPercentage },
                new() { Value = specialOffersModel.Code },
                new() { Value = specialOffersModel.PriceType },
                new() { Value = storeId },
                new() { Value = specialOffersModel.Id }
            }
        };

        await using var specialOfferReader = specialOfferUpdateCommand.ExecuteReader();

        if (!specialOfferReader.HasRows)
            return null;

        if (!await specialOfferReader.ReadAsync())
            return null;

        var specialOffer = specialOfferReaderToSpecialOffer(specialOfferReader);
        return specialOffer;
    }

    /// <summary>
    /// Deletes a special offer by its ID.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="id">The ID of the special offer to delete.</param>
    /// <param name="author">The staff member performing the deletion.</param>
    /// <returns>The deleted special offer, or null if the deletion failed.</returns>
    public async Task<SpecialOffer?> DeleteByIdAsync(long storeId, long id, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ??
            throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE special_offer
            SET deleted_by = $1,
                deleted_at = $2
            WHERE 
                store_id = $3
            AND
                id = $4
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

        await using var reader = command.ExecuteReader();

        if (!reader.HasRows)
            return null;

        if (!await reader.ReadAsync())
            return null;

        return specialOfferReaderToSpecialOffer(reader);
    }

    /// <summary>
    /// Deletes multiple special offers by their IDs.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="ids">The list of IDs of the special offers to delete.</param>
    /// <param name="author">The staff member performing the deletion.</param>
    /// <returns>A list of deleted special offers.</returns>
    public async Task<List<SpecialOffer>> DeleteByIdsAsync(long storeId, List<long> ids, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE special_offer
            SET deleted_by = $1,
                deleted_at = $2
            WHERE 
                store_id = $3
            AND
                id = ANY($4)
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = author.CompositeUsername },
                new() { Value = DateTime.Now },
                new() { Value = storeId },
                new() { Value = ids.ToArray() }
            }
        };

        await using var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            var specialOffers = new List<SpecialOffer>();

            while (await reader.ReadAsync())
            {
                specialOffers.Add(specialOfferReaderToSpecialOffer(reader));
            }

            return specialOffers;
        }

        return new();
    }

    /// <summary>
    /// Counts the number of special offers that match the given filter criteria.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="specialOffersFilterDto">The filter criteria for special offers.</param>
    /// <returns>The count of special offers that match the filter criteria.</returns>
    public async Task<long> CountAsync(long storeId, SpecialOfferFilterDto specialOffersFilterDto)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        var query = new StringBuilder(@"
            SELECT COUNT(*) 
            FROM special_offer 
            WHERE 
                deleted_at IS NULL
            AND
                store_id = $1");

        var parameters = new List<NpgsqlParameter>();
        parameters.Add(new() { Value = storeId });

        var index = 2;
        if (!string.IsNullOrEmpty(specialOffersFilterDto.Code))
        {
            query.Append(" AND code ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = specialOffersFilterDto.Code });
            index++;
        }

        if (!string.IsNullOrEmpty(specialOffersFilterDto.Name))
        {
            query.Append(" AND name ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = specialOffersFilterDto.Name });
            index++;
        }

        if (!string.IsNullOrEmpty(specialOffersFilterDto.Desc))
        {
            query.Append(" AND desc ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = specialOffersFilterDto.Desc });
            index++;
        }

        if (specialOffersFilterDto.CategoryId.HasValue)
        {
            query.Append(" AND category_id = $" + index);
            parameters.Add(new() { Value = specialOffersFilterDto.CategoryId });
            index++;
        }

        if (specialOffersFilterDto.ProductId.HasValue)
        {
            query.Append(" AND product_id = $" + index);
            parameters.Add(new() { Value = specialOffersFilterDto.ProductId });
            index++;
        }

        if (!string.IsNullOrEmpty(specialOffersFilterDto.Status))
        {
            query.Append(" AND status = $" + index);
            parameters.Add(new() { Value = specialOffersFilterDto.Status });
            index++;
        }

        await using var command = new NpgsqlCommand(query.ToString(), connection);
        command.Parameters.AddRange(parameters.ToArray());

        return await command.ExecuteScalarAsync() as long? ?? 0;
    }

    /// <summary>
    /// Converts a data reader to a SpecialOffer object.
    /// </summary>
    /// <param name="reader">The data reader containing special offer data.</param>
    /// <returns>A SpecialOffer object populated with data from the reader.</returns>
    private SpecialOffer specialOfferReaderToSpecialOffer(NpgsqlDataReader reader)
    {
        SpecialOffer specialOffer = new SpecialOffer
        {
            // BaseModel fields
            Id = reader.GetInt64(reader.GetOrdinal("id")),
            Code = reader.GetString(reader.GetOrdinal("code")),
            CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),   // ensured not null in creation
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")), // ensured not null in creation
            // Category fields
            StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            Description = reader.GetString(reader.GetOrdinal("description")),
            Type = reader.GetString(reader.GetOrdinal("type")),
            PriceType = reader.GetString(reader.GetOrdinal("price_type")),
            ItemsSold = reader.GetDecimal(reader.GetOrdinal("items_sold")),
            MaxItems = reader.GetDecimal(reader.GetOrdinal("max_items")),
            Status = reader.GetString(reader.GetOrdinal("status")),
        };

        // Handle nullable columns
        specialOffer.UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by")) ?
            null : reader.GetString(reader.GetOrdinal("updated_by"));
        specialOffer.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ?
            null : reader.GetDateTime(reader.GetOrdinal("updated_at"));
        specialOffer.DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by")) ?
            null : reader.GetString(reader.GetOrdinal("deleted_by"));
        specialOffer.DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) ?
            null : reader.GetDateTime(reader.GetOrdinal("deleted_at"));
        specialOffer.CategoryId = reader.IsDBNull(reader.GetOrdinal("category_id")) ?
            null : reader.GetInt64(reader.GetOrdinal("category_id"));
        specialOffer.ProductId = reader.IsDBNull(reader.GetOrdinal("product_id")) ?
            null : reader.GetInt64(reader.GetOrdinal("product_id"));
        specialOffer.StartDate = reader.IsDBNull(reader.GetOrdinal("start_date")) ?
            null : reader.GetDateTime(reader.GetOrdinal("start_date"));
        specialOffer.EndDate = reader.IsDBNull(reader.GetOrdinal("end_date")) ?
            null : reader.GetDateTime(reader.GetOrdinal("end_date"));
        specialOffer.DiscountPrice = reader.IsDBNull(reader.GetOrdinal("discount_price")) ?
            null : reader.GetDecimal(reader.GetOrdinal("discount_price"));
        specialOffer.DiscountPercentage = reader.IsDBNull(reader.GetOrdinal("discount_percentage")) ?
            null : reader.GetDecimal(reader.GetOrdinal("discount_percentage"));
        return specialOffer;
    }

}