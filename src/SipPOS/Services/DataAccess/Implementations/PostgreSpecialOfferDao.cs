using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.DataTransfer.General;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;
using SipPOS.DataTransfer.Entity;
using Npgsql;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text;
using NpgsqlTypes;

namespace SipPOS.Services.DataAccess.Implementations;


public class PostgreSpecialOfferDao : ISpecialOfferDao
{
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
                status
            )
            VALUES 
                ($1, $2, $3, $4, $5, $6, $7)
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
                new() { Value = specialOffersModel.CategoryId },
                new() { Value = specialOffersModel.Status }
            }
        };
        
        await using var specialOfferReader = specialOfferInsertCommand.ExecuteReader();
        
        if (!specialOfferReader.HasRows)
            return null;

        await specialOfferReader.ReadAsync();

        return specialOfferReaderToSpecialOffer(specialOfferReader);
    }

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
                status = $7
            WHERE 
                store_id = $8
            AND
                id = $9
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
                new() { Value = specialOffersModel.CategoryId },
                new() { Value = specialOffersModel.Status },
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

    private SpecialOffer specialOfferReaderToSpecialOffer(NpgsqlDataReader reader)
    {
        SpecialOffer specialOffer = new SpecialOffer
        {
            // BaseModel fields
            Id = reader.GetInt64(reader.GetOrdinal("id")),
            CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),   // ensured not null in creation
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")), // ensured not null in creation
            // Category fields
            StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            Description = reader.GetString(reader.GetOrdinal("description")),
            ItemsSold = reader.GetInt64(reader.GetOrdinal("items_sold")),
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
        
        return specialOffer;
    }

}