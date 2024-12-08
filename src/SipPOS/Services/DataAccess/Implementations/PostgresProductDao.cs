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

namespace SipPOS.Services.DataAccess.Implementations;

/// <summary>
/// Mock implementation of the IProductDao interface for testing purposes.
/// </summary>
public class PostgresProductDao : IProductDao
{

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>The deleted product if found; otherwise, null.</returns>
    public async Task<Product?> DeleteByIdAsync(long id, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE product
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

        await using var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                return ReaderToProduct(reader);
            }
        }

        return null;

    }

    /// <summary>
    /// Deletes multiple products by their IDs.
    /// </summary>
    /// <param name="ids">The list of IDs of the products to delete.</param>
    /// <returns>A list of deleted products.</returns>
    public async Task<List<Product>> DeleteByIdsAsync(List<long> ids, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE product
            SET deleted_by = $1,
                deleted_at = $2,
            WHERE id = ANY($3)
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

        await using var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            var products = new List<Product>();

            while (await reader.ReadAsync())
            {
                products.Add(ReaderToProduct(reader));
            }

            return products;
        }

        return new ();
    }

    /// <summary>
    /// Retrieves all products that are not marked as deleted.
    /// </summary>
    /// <returns>A list of all available products.</returns>
    public async Task<List<Product>> GetAllAsync()
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            SELECT *
            FROM product
            WHERE deleted_at IS NULL
        ", connection);

        await using var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            var products = new List<Product>();

            while (await reader.ReadAsync())
            {
                products.Add(ReaderToProduct(reader));
            }

            return products;
        }

        return new ();
    }

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    public async Task<Product?> GetByIdAsync(long id)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>() ?? throw new InvalidOperationException("Failed to open database connection.");

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            SELECT *
            FROM product
            WHERE id = $1
        ", connection)
        {
            Parameters = { new() { Value = id } }
        };

        await using var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                return ReaderToProduct(reader);
            }
        }

        return null;

    }

    /// <summary>
    /// Inserts a new product.
    /// </summary>
    /// <param name="product">The product to insert.</param>
    /// <returns>The inserted product.</returns>
    public async Task<Product?> InsertAsync(Product productDto)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var command = new NpgsqlCommand(@"
            INSERT INTO product (name, desc, price, category_id, status, image_urls, created_by, created_at)
            VALUES ($1, $2, $3, $4, $5, $6, $7, $8)
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = productDto.Name },
                new() { Value = productDto.Desc },
                new() { Value = productDto.Price },
                new() { Value = productDto.CategoryId },
                new() { Value = productDto.Status },
                new() { Value = JsonSerializer.Serialize(productDto.ImageUrls) },
                new() { Value = productDto.CreatedBy },
                new() { Value = productDto.CreatedAt }
            }
        };

        await using var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                return ReaderToProduct(reader);
            }
        }

        return null;
    }

    /// <summary>
    /// Updates an existing product by its ID.
    /// </summary>
    /// <param name="productDto">The product with updated information.</param>
    /// <returns>The updated product if found; otherwise, null.</returns>
    public async Task<Product?> UpdateByIdAsync(Product productDto)
    {
       var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE product
            SET name = $1,
                desc = $2,
                price = $3,
                category_id = $4,
                status = $5,
                image_urls = $6,
                updated_by = $7,
                updated_at = $8
            WHERE id = $9
            RETURNING *
        ", connection)
        {
            Parameters =
            {
                new() { Value = productDto.Name },
                new() { Value = productDto.Desc },
                new() { Value = productDto.Price },
                new() { Value = productDto.CategoryId },
                new() { Value = productDto.Status },
                new() { Value = JsonSerializer.Serialize(productDto.ImageUrls) },
                new() { Value = productDto.UpdatedBy },
                new() { Value = productDto.UpdatedAt },
                new() { Value = productDto.Id }
            }
        };

        await using var reader = command.ExecuteReader();

        if (reader.HasRows)
        {
            if (await reader.ReadAsync())
            {
                return ReaderToProduct(reader);
            }
        }

        return null;
    }

    /// <summary>
    /// Counts the total number of products.
    /// </summary>
    /// <returns>The total number of products.</returns>
    public async Task<long> CountAsync(ProductFilterDto productFilterDto)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        var query = new StringBuilder("SELECT COUNT(*) FROM product WHERE deleted_at IS NULL");

        var parameters = new List<NpgsqlParameter>();

        int index = 1;
        if (!string.IsNullOrEmpty(productFilterDto.Name))
        {
            query.Append(" AND name ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = productFilterDto.Name });
            index++;
        }

        if (!string.IsNullOrEmpty(productFilterDto.Desc))
        {
            query.Append(" AND desc ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = productFilterDto.Desc });
            index++;
        }

        if (productFilterDto.CategoryId.HasValue)
        {
            query.Append(" AND category_id = $" + index);
            parameters.Add(new() { Value = productFilterDto.CategoryId });
            index++;
        }

        if (!string.IsNullOrEmpty(productFilterDto.Status))
        {
            query.Append(" AND status = $" + index);
            parameters.Add(new() { Value = productFilterDto.Status });
            index++;
        }

        await using var command = new NpgsqlCommand(query.ToString(), connection);
        command.Parameters.AddRange(parameters.ToArray());

        return command.ExecuteScalar() as long? ?? 0;
    }

    /// <summary>
    /// Perform a search for products in a pagination.
    /// </summary>
    /// <param name="productFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorting options to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of products per page.</param>
    /// <returns>A pagination object containing the search results.</returns>
    public async Task<Pagination<Product>> SearchAsync(ProductFilterDto productFilterDto, SortDto sortDto, int page, int perPage)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        var query = new StringBuilder("SELECT * FROM product WHERE deleted_at IS NULL");

        var parameters = new List<NpgsqlParameter>();

        int index = 1;
        if (!string.IsNullOrEmpty(productFilterDto.Name))
        {
            query.Append(" AND name ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = productFilterDto.Name });
            index++;
        }

        if (!string.IsNullOrEmpty(productFilterDto.Desc))
        {
            query.Append(" AND desc ILIKE '%' || $" + index + " || '%'");
            parameters.Add(new() { Value = productFilterDto.Desc });
            index++;
        }

        if (productFilterDto.CategoryId.HasValue)
        {
            query.Append(" AND category_id = $" + index);
            parameters.Add(new() { Value = productFilterDto.CategoryId });
            index++;
        }

        if (!string.IsNullOrEmpty(productFilterDto.Status))
        {
            query.Append(" AND status = $" + index);
            parameters.Add(new() { Value = productFilterDto.Status });
            index++;
        }

        query.Append($" ORDER BY {sortDto.SortBy} {sortDto.SortType} LIMIT $" + index + " OFFSET $" + (index + 1));
        parameters.Add(new() { Value = perPage });
        parameters.Add(new() { Value = (page - 1) * perPage });

        await using var command = new NpgsqlCommand(query.ToString(), connection);
        command.Parameters.AddRange(parameters.ToArray());

        await using var reader = command.ExecuteReader();

        long totalRecord = await CountAsync(productFilterDto);

        if (reader.HasRows)
        {
            var products = new List<Product>();

            while (await reader.ReadAsync())
            {
                products.Add(ReaderToProduct(reader));
            }

            return new Pagination<Product>
            {
                Data = products,
                Page = page,
                PerPage = perPage,
                TotalRecord = totalRecord,
                TotalPage = (int)Math.Ceiling((double)totalRecord / perPage)
            };
        }

        return new Pagination<Product>
        {
            Data = new List<Product>(),
            Page = page,
            PerPage = perPage,
            TotalRecord = 0,
            TotalPage = 0
        };
    }

    private Product ReaderToProduct(NpgsqlDataReader reader)
    {
        Product product = new Product
        {
            // BaseModel fields
            Id = reader.GetInt64(reader.GetOrdinal("id")),
            CreatedBy = reader.GetString(reader.GetOrdinal("created_by")),   // ensured not null in creation
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")), // ensured not null in creation
            // Category fields
            Price = reader.GetDouble(reader.GetOrdinal("price")),
            CategoryId = reader.GetInt64(reader.GetOrdinal("category_id")),
            Name = reader.GetString(reader.GetOrdinal("name")),
            Status = reader.GetString(reader.GetOrdinal("status")),
        };

        // Handle nullable columns
        product.UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by")) == true ?
            null : reader.GetString(reader.GetOrdinal("updated_by"));
        product.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) == true ?
            null : reader.GetDateTime(reader.GetOrdinal("updated_at"));
        product.DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by")) == true ?
            null : reader.GetString(reader.GetOrdinal("deleted_by"));
        product.DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) == true ?
            null : reader.GetDateTime(reader.GetOrdinal("deleted_at"));
        product.Desc = reader.IsDBNull(reader.GetOrdinal("desc")) == true ?
            null : reader.GetString(reader.GetOrdinal("desc"));
        product.ImageUrls = reader.IsDBNull(reader.GetOrdinal("image_urls")) == true ?
            new List<string>() : new List<string>(reader.GetFieldValue<string[]>(reader.GetOrdinal("image_urls")));
        return product;
    }
}
