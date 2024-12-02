using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.DataTransfer.General;
using SipPOS.Services.DataAccess.Interfaces;
using Npgsql;
using SipPOS.DataTransfer.Entity;
using SipPOS.Services.General.Interfaces;
using System.Text.Json;
using System.Text;

namespace SipPOS.Services.DataAccess.Implementations;

/// <summary>
/// Mock implementation of the ICategoryDao interface for testing purposes.
/// Providing mock data for a few Category objects.
/// </summary>
public class PostgresCategoryDao : ICategoryDao
{

    /// <summary>
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to delete.</param>
    /// <returns>The deleted category if found; otherwise, null.</returns>
    public async Task<Category?> DeleteByIdAsync(long id, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE category
            SET deleted_by = $1,
                deleted_at = $2,
            WHERE id = $3
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = author.CompositeUsername },
                new() { Value = DateTime.Now },
                new() { Value = id }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                return ReaderToCategory(reader);
            }
        }

        return null;
    }

    /// <summary>
    /// Deletes multiple categories by their IDs.
    /// </summary>
    /// <param name="ids">The list of IDs of the categories to delete.</param>
    /// <returns>A list of deleted categories.</returns>
    public async Task<List<Category>> DeleteByIdsAsync(List<long> ids, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE category
            SET deleted_by = $1,
                deleted_at = $2,
            WHERE store_id = $3 AND id = ANY($4)
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = author.CompositeUsername },
                new() { Value = DateTime.Now },
                new() { Value = ids.ToArray() }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            List<Category> categoryList = new();

            while (await reader.ReadAsync())
            {
                categoryList.Add(ReaderToCategory(reader));
            }

            return categoryList;
        }

        return new();
    }

    /// <summary>
    /// Retrieves all categories that are not marked as deleted.
    /// </summary>
    /// <returns>A list of all available categories.</returns>
    public async Task<List<Category>> GetAllAsync()
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");
        await using var command = new NpgsqlCommand(@"
            SELECT *
            FROM category
            WHERE deleted_at IS NULL
        ", connection);

        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            List<Category> categories = new();

            while (await reader.ReadAsync())
            {
                categories.Add(ReaderToCategory(reader));
            }

            return categories;
        }
        return new();
    }

    /// <summary>
    /// Retrieves a category by its ID.
    /// </summary>
    /// <param name="id">The ID of the category to retrieve.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    public async Task<Category?> GetByIdAsync(long id)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            SELECT * FROM category 
            WHERE id = $1
        ", connection)
        {
            Parameters =
            {
                new() { Value = id }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                return ReaderToCategory(reader);
            }
        }

        return null;
    }

    /// <summary>
    /// Inserts a new category.
    /// </summary>
    /// <param name="categoryDto">The category to insert.</param>
    /// <returns>The inserted category.</returns>
    public async Task<(long id, Category? dto)> InsertAsync(Category categoryDto)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        using var command = new NpgsqlCommand(@"
            INSERT INTO category (name, desc, status, image_urls, created_by, created_at)
            VALUES ($1, $2, $3, $4, $5, $6)
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = categoryDto.Name },
                new() { Value = categoryDto.Desc },
                new() { Value = categoryDto.Status },
                new() { Value = JsonSerializer.Serialize(categoryDto.ImageUrls) },
                new() { Value = categoryDto.CreatedBy },
                new() { Value = categoryDto.CreatedAt }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();
        if (reader.HasRows)
        {
            await reader.ReadAsync();
            return (reader.GetInt64(reader.GetOrdinal("id")), ReaderToCategory(reader));
        }

        return (0, null);
    }

    /// <summary>
    /// Updates an existing category by its ID.
    /// </summary>
    /// <param name="categoryDto">The category with updated information.</param>
    /// <returns>The updated category if found; otherwise, null.</returns>
    public async Task<Category?> UpdateByIdAsync(Category categoryDto)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ;

        using var command = new NpgsqlCommand(@"
            UPDATE category
            SET name = $1,
                desc = $2,
                status = $3,
                image_urls = $4,
                updated_by = $5,
                updated_at = $6
            WHERE id = $7
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = categoryDto.Name },
                new() { Value = categoryDto.Desc },
                new() { Value = categoryDto.Status },
                new() { Value = JsonSerializer.Serialize(categoryDto.ImageUrls) },
                new() { Value = categoryDto.UpdatedBy },
                new() { Value = categoryDto.UpdatedAt },
                new() { Value = categoryDto.Id }
            }
        };

        await using var reader = await command.ExecuteReaderAsync();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                return ReaderToCategory(reader);
            }
        }

        return null;
    }

    /// <summary>
    /// Counts the total number of categories.
    /// </summary>
    /// <returns>The total number of categories.</returns>
    public async Task<long> CountAsync(CategoryFilterDto categoryFilterDto)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        var query = new StringBuilder("SELECT COUNT(*) FROM category WHERE deleted_at IS NULL");

        var parameters = new List<NpgsqlParameter>();

        int index = 1;
        if (categoryFilterDto.Name != null)
        {
            query.Append(" name ILIKE '%' || $" + index++ + " || '%'");
            parameters.Add(new() { Value = categoryFilterDto.Name });
            index++;
        }

        if (categoryFilterDto.Desc != null)
        {
            query.Append(" desc ILIKE '%' || $" + index++ + " || '%'");
            parameters.Add(new() { Value = categoryFilterDto.Desc });
            index++;
        }

        if (categoryFilterDto.Status != null)
        {
            query.Append(" status = $" + index++);
            parameters.Add(new() { Value = categoryFilterDto.Status });
            index++;
        }

        await using var command = new NpgsqlCommand(query.ToString(), connection);
        command.Parameters.AddRange(parameters.ToArray());

        return command.ExecuteScalar() as long? ?? 0;
    }

    /// <summary>
    /// Searches for categories with pagination.
    /// </summary>
    /// <param name="categoryFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorting options to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of categories per page.</param>
    /// <returns>A pagination object containing the search results.</returns>
    public async Task<Pagination<Category>> SearchAsync(CategoryFilterDto categoryFilterDto, SortDto sortDto, int page, int perPage)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        var query = new StringBuilder("SELECT * FROM category WHERE deleted_at IS NULL");

        var parameters = new List<NpgsqlParameter>();

        int index = 1;
        if (categoryFilterDto.Name != null)
        {
            query.Append(" AND name ILIKE '%' || $" + index++ + " || '%'");
            parameters.Add(new() { Value = categoryFilterDto.Name });
            index++;
        }

        if (categoryFilterDto.Desc != null)
        {
            query.Append(" AND desc ILIKE '%' || $" + index++ + " || '%'");
            parameters.Add(new() { Value = categoryFilterDto.Desc });
            index++;
        }

        if (categoryFilterDto.Status != null)
        {
            query.Append(" AND status = $" + index++);
            parameters.Add(new() { Value = categoryFilterDto.Status });
            index++;
        }

        query.Append($" ORDER BY {sortDto.SortBy} {sortDto.SortType} LIMIT $" + index + " OFFSET $" + (index + 1));
        parameters.Add(new() { Value = perPage });
        parameters.Add(new() { Value = (page - 1) * perPage });

        using var command = new NpgsqlCommand(query.ToString(), connection);
        command.Parameters.AddRange(parameters.ToArray());

        using var reader = command.ExecuteReader();


        long totalRecords = await CountAsync(categoryFilterDto);

        if (reader.HasRows)
        {
            List<Category> categoryList = new();

            while (reader.Read())
            {
                categoryList.Add(ReaderToCategory(reader));
            }

            return new Pagination<Category>
            {
                Data = categoryList,
                Page = page,
                PerPage = perPage,
                TotalRecord = totalRecords,
                TotalPage = (int)Math.Ceiling((double)totalRecords / perPage)
            };
        }

        return new Pagination<Category>();
    }

    private Category ReaderToCategory(NpgsqlDataReader reader)
    {
        Category category = new Category()
        {
            // BaseModel fields
            Id = reader.GetInt64(reader.GetOrdinal("id")),
            CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),   // ensured not null in creation
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")), // ensured not null in creation
            // Category fields
            Name = reader.GetString(reader.GetOrdinal("name")),
            Status = reader.GetString(reader.GetOrdinal("status")),
        };

        // Handle nullable columns
        category.UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by")) == true ?
            null : reader.GetString(reader.GetOrdinal("updated_by"));
        category.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) == true ?
            null : reader.GetDateTime(reader.GetOrdinal("updated_at"));
        category.DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by")) == true ?
            null : reader.GetString(reader.GetOrdinal("deleted_by"));
        category.DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) == true ?
            null : reader.GetDateTime(reader.GetOrdinal("deleted_at"));
        category.Desc = reader.IsDBNull(reader.GetOrdinal("desc")) == true ?
            null : reader.GetString(reader.GetOrdinal("desc"));
        category.ImageUrls = reader.IsDBNull(reader.GetOrdinal("image_urls")) == true ?
            new List<string>() : new List<string>(reader.GetFieldValue<string[]>(reader.GetOrdinal("image_urls")));
        return category;
    }
}
