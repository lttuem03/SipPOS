using System.Text;

using SipPOS.Models.Entity;
using SipPOS.Models.General;
using SipPOS.DataTransfer.General;
using SipPOS.Services.DataAccess.Interfaces;
using SipPOS.Services.General.Interfaces;

using Npgsql;

namespace SipPOS.Services.DataAccess.Implementations;

/// <summary>
/// Mock implementation of the ICategoryDao interface for testing purposes.
/// Providing mock data for a few Category objects.
/// </summary>
public class PostgreCategoryDao : ICategoryDao
{
    /// <summary>
    /// Inserts a new category into a store.
    /// </summary>
    /// <param name="storeId">The id of the store to insert category into.</param>
    /// <param name="category">The category to insert.</param>
    /// <returns>The inserted category if successful; otherwise, null.</returns>
    public async Task<Category?> InsertAsync(long storeId, Category categoryModel)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        using var command = new NpgsqlCommand(@"
            INSERT INTO category 
            (
                store_id,
                created_by,
                created_at,
                name,
                description
            )
            VALUES 
                ($1, $2, $3, $4, $5)
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = categoryModel.CreatedBy },
                new() { Value = categoryModel.CreatedAt },
                new() { Value = categoryModel.Name },
                new() { Value = categoryModel.Description }
            }
        };

        await using var reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            await reader.ReadAsync();
            return ReaderToCategory(reader);
        }

        return null;
    }


    /// <summary>
    /// Retrieves all categories that marked "not deleted" of a store.
    /// </summary>
    /// <param name="storeId">The id of the store to get categories from.</param>
    /// <returns>A list of all categories.</returns>
    public async Task<List<Category>> GetAllAsync(long storeId)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            SELECT *
            FROM category
            WHERE 
                deleted_at IS NULL
            AND
                store_id = $1
        ", connection)
        {
            Parameters =
            {
                new() { Value = storeId }
            }
        };

        await using var reader = command.ExecuteReader();

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
    /// <param name="storeId">The id of the store to get categories from.</param>
    /// <param name="id">The ID of the category to retrieve.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    public async Task<Category?> GetByIdAsync(long storeId, long id)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            SELECT * FROM category 
            WHERE store_id = $1 AND id = $2
        ", connection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = id }
            }
        };

        await using var reader = command.ExecuteReader();

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
    /// Searches for categories with pagination.
    /// </summary>
    /// <param name="storeId">The id of the store to search.</param>
    /// <param name="categoryFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorting options to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of categories per page.</param>
    /// <returns>A pagination object containing the search results.</returns>
    public async Task<Pagination<Category>> GetWithPaginationAsync
    (
        long storeId,
        CategoryFilterDto categoryFilterDto, 
        SortDto sortDto, 
        int page, 
        int perPage
    )
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        var query = new StringBuilder(@"
            SELECT * 
            FROM category 
            WHERE deleted_at IS NULL
                AND store_id = $1
        ");

        var parameters = new List<NpgsqlParameter>();
        parameters.Add(new() { Value = storeId });

        var index = 2;
        if (categoryFilterDto.Name != null)
        {
            query.Append(" AND name ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = categoryFilterDto.Name });
            index++;
        }

        if (categoryFilterDto.Desc != null)
        {
            query.Append(" AND desc ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = categoryFilterDto.Desc });
            index++;
        }

        if (categoryFilterDto.Status != null)
        {
            query.Append(" AND status = $" + index);
            parameters.Add(new() { Value = categoryFilterDto.Status });
            index++;
        }

        query.Append($" ORDER BY {sortDto.SortBy} {sortDto.SortType} LIMIT $" + index + " OFFSET $" + (index + 1));
        parameters.Add(new() { Value = perPage });
        parameters.Add(new() { Value = (page - 1) * perPage < 0 ? 0 : (page - 1) * perPage });

        using var command = new NpgsqlCommand(query.ToString(), connection);
        command.Parameters.AddRange(parameters.ToArray());

        using var reader = command.ExecuteReader();

        var totalRecord = await CountAsync(storeId, categoryFilterDto);

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
                TotalRecord = totalRecord,
                TotalPage = (int)Math.Ceiling((double)totalRecord / perPage)
            };
        }

        return new Pagination<Category>();
    }

    /// <summary>
    /// Updates an existing category by its ID.
    /// </summary>
    /// <param name="storeId">The id of the store to update category from.</param>
    /// <param name="categoryModel">The category with updated information.</param>
    /// <returns>The updated category if found; otherwise, null.</returns>
    public async Task<Category?> UpdateByIdAsync(long storeId, Category categoryModel)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        using var command = new NpgsqlCommand(@"
            UPDATE category
            SET 
                name = $1,
                description = $2,
                updated_by = $3,
                updated_at = $4
            WHERE store_id = $5 AND id = $6
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = categoryModel.Name },
                new() { Value = categoryModel.Description },
                new() { Value = categoryModel.UpdatedBy },
                new() { Value = categoryModel.UpdatedAt },
                new() { Value = storeId },
                new() { Value = categoryModel.Id }
            }
        };

        await using var reader = command.ExecuteReader();

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
    /// Deletes a category by its ID.
    /// </summary>
    /// <param name="storeId">The id of the store to delete categories from.</param>
    /// <param name="id">The ID of the category to delete.</param>
    /// <param name="author">The author of the operation.</param>
    /// <returns>The deleted category if successful; otherwise, null.</returns>
    public async Task<Category?> DeleteByIdAsync(long storeId, long id, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE category
            SET 
                deleted_by = $1,
                deleted_at = $2
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

        await using var reader = command.ExecuteReader();

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
    /// <param name="storeId">The id of the store to delete categories from.</param>
    /// <param name="ids">The IDs of the categories to delete.</param>
    /// <param name="author">The author of the operation.</param>
    /// <returns>A list of deleted categories.</returns>
    public async Task<List<Category>> DeleteByIdsAsync(long storeId, List<long> ids, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE category
            SET 
                deleted_by = $1,
                deleted_at = $2
            WHERE store_id = $3 AND id = ANY($4)
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
    /// Counts the total number of categories that fit the filters.
    /// </summary>
    /// <param name="storeId">The id of the store to count in.</param>
    /// <param name="categoryFilterDto">The filter to apply to the count query.</param>
    /// <returns>The total number of categories that fit the filters.</returns>
    public async Task<long> CountAsync(long storeId, CategoryFilterDto categoryFilterDto)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        var query = new StringBuilder(@"
            SELECT COUNT(*) 
            FROM category 
            WHERE deleted_at IS NULL AND store_id = $1
        ");

        var parameters = new List<NpgsqlParameter>();
        parameters.Add(new() { Value = storeId });

        var index = 2;
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

        await using var command = new NpgsqlCommand(query.ToString(), connection);
        command.Parameters.AddRange(parameters.ToArray());

        return command.ExecuteScalar() as long? ?? 0;
    }

    /// <summary>
    /// Converts a data reader object to a Category object.
    /// </summary>
    /// <param name="reader">The data reader containing category data.</param>
    /// <returns>A Category object populated with data from the reader.</returns>
    private Category ReaderToCategory(NpgsqlDataReader reader)
    {
        Category category = new Category()
        {
            // BaseModel fields
            Id = reader.GetInt64(reader.GetOrdinal("id")),
            CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),   // ensured not null in creation
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")), // ensured not null in creation
            // Category fields
            StoreId = reader.GetInt64(reader.GetOrdinal("store_id")),
            Name = reader.GetString(reader.GetOrdinal("name"))
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
        category.Description = reader.IsDBNull(reader.GetOrdinal("description")) == true ?
            null : reader.GetString(reader.GetOrdinal("description"));

        return category;
    }
}
