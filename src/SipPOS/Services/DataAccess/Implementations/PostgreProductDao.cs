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

/// <summary>
/// Mock implementation of the IProductDao interface for testing purposes.
/// </summary>
public class PostgreProductDao : IProductDao
{
    /// <summary>
    /// Inserts a new product.
    /// </summary>
    /// <param name="product">The product to insert.</param>
    /// <returns>The inserted product.</returns>
    public async Task<Product?> InsertAsync(long storeId, Product productDto)
    {
        if (productDto.ProductOptions.Count == 0)
            throw new InvalidOperationException("Product must have at least one option.");
        
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        // INSERT INTO product
        using var productInsertConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;
        
        await using var productInsertCommand = new NpgsqlCommand(@"
            INSERT INTO product 
            (
                store_id, 
                created_by, 
                created_at, 
                name, 
                description, 
                category_store_id, 
                category_id, 
                image_uris, 
                status
            )
            VALUES 
                ($1, $2, $3, $4, $5, $6, $7, $8, $9)
            RETURNING *
        ", productInsertConnection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = productDto.CreatedBy },
                new() { Value = productDto.CreatedAt },
                new() { Value = productDto.Name },
                new() { Value = productDto.Description },
                new() { Value = productDto.CategoryId != null ? storeId : null },
                new() { Value = productDto.CategoryId },
                new() { Value = productDto.ImageUris },
                new() { Value = productDto.Status }
            }
        };
        
        await using var productReader = productInsertCommand.ExecuteReader();
        
        // Insert had failed
        if (!productReader.HasRows)
            return null;

        await productReader.ReadAsync();
        var product = productReaderToProduct(productReader);

        // INSERT INTO product_option
        var productOptions = new List<(string name, decimal price)>();

        foreach (var option in productDto.ProductOptions)
        {
            using var productOptionInsertConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;
            
            await using var productOptionInsertCommand = new NpgsqlCommand(@"
                INSERT INTO product_option
                (
                    product_id,
                    store_id,
                    name,
                    price
                )
                VALUES 
                    ($1, $2, $3, $4)
                RETURNING *
            ", productOptionInsertConnection)
            {
                Parameters = 
                {
                    new() { Value = product.Id },
                    new() { Value = product.StoreId },  
                    new() { Value = option.name },
                    new() { Value = option.price }
                }
            };

            await using var productOptionReader = productOptionInsertCommand.ExecuteReader();
            await productOptionReader.ReadAsync();

            productOptions.Add(productOptionReaderToProductOption(productOptionReader));
        }

        product.ProductOptions = productOptions;

        return product;
    }

    /// <summary>
    /// Retrieves all products of a store (that are not marked as 'deleted'.
    /// </summary>
    /// <param name="storeId">The store id to get products from.</param>
    /// <returns>A list of all products.</returns>
    public async Task<List<Product>> GetAllAsync(long storeId)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        // SELECT product
        using var productSelectConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ??
            throw new InvalidOperationException("Failed to open database connection.");

        await using var productSelectCommand = new NpgsqlCommand(@"
            SELECT *
            FROM product
            WHERE 
                deleted_at IS NULL 
            AND 
                store_id = $1
        ", productSelectConnection)
        {
            Parameters = 
            {
                new() { Value = storeId }
            }
        };

        await using var productReader = productSelectCommand.ExecuteReader();

        if (productReader.HasRows)
        {
            var products = new List<Product>();

            while (await productReader.ReadAsync())
            {
                var product = productReaderToProduct(productReader);

                // SELECT product_option
                using var productOptionSelectConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

                await using var productOptionSelectCommand = new NpgsqlCommand(@"
                    SELECT *
                    FROM product_option
                    WHERE 
                        product_id = $1
                    AND
                        store_id = $2
                ", productOptionSelectConnection)
                {
                    Parameters =
                    {
                        new() { Value = product.Id },
                        new() { Value = product.StoreId }
                    }
                };

                //try
                //{
                    await using var productOptionReader = productOptionSelectCommand.ExecuteReader();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine($"An error occurred: {ex.Message}");
                //    throw; // Re-throw the exception if you want it to propagate
                //}

                if (!productOptionReader.HasRows)
                {
                    // if the product has no option (which
                    // is not likely due to the creation
                    // pipeline), it does not get returned at all
                    continue;
                }
                
                var productOptions = new List<(string name, decimal price)>();
                
                while (await productOptionReader.ReadAsync())
                    productOptions.Add(productOptionReaderToProductOption(productOptionReader));
                
                product.ProductOptions = productOptions;

                products.Add(product);
            }

            return products;
        }

        return new();
    }

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product if found; otherwise, null.</returns>
    public async Task<Product?> GetByIdAsync(long storeId, long id)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>() ?? throw new InvalidOperationException("Failed to open database connection.");

        using var productSelectConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        // SELECT product
        await using var productSelectCommand = new NpgsqlCommand(@"
            SELECT *
            FROM product
            WHERE store_id = $1 AND id = $2
        ", productSelectConnection)
        {
            Parameters = 
            {
                new() { Value = storeId },
                new() { Value = id }
            }
        };

        await using var productReader = productSelectCommand.ExecuteReader();

        if (productReader.HasRows)
        {
            if (await productReader.ReadAsync())
            {
                var product = productReaderToProduct(productReader);

                // SELECT product_option
                using var productOptionSelectConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

                await using var productOptionSelectCommand = new NpgsqlCommand(@"
                    SELECT *
                    FROM product_option
                    WHERE 
                        product_id = $1
                        store_id = $2
                ", productSelectConnection)
                {
                    Parameters =
                    {
                        new() { Value = product.Id },
                        new() { Value = product.StoreId }
                    }
                };

                await using var productOptionReader = productOptionSelectCommand.ExecuteReader();

                if (!productOptionReader.HasRows)
                {
                    // if the product has no option (which
                    // is not likely due to the creation
                    // pipeline), it does not get returned at all
                    return null;
                }

                var productOptions = new List<(string name, decimal price)>();

                while (await productOptionReader.ReadAsync())
                    productOptions.Add(productOptionReaderToProductOption(productOptionReader));

                product.ProductOptions = productOptions;
            }
        }

        return null;
    }

    /// <summary>
    /// Perform a search for products in a pagination.
    /// </summary>
    /// <param name="productFilterDto">The filters to apply.</param>
    /// <param name="sortDto">The sorting options to apply.</param>
    /// <param name="page">The page number to retrieve.</param>
    /// <param name="perPage">The number of products per page.</param>
    /// <returns>A pagination object containing the search results.</returns>
    public async Task<Pagination<Product>> GetWithPaginationAsync
    (
        long storeId,
        ProductFilterDto productFilterDto, 
        SortDto sortDto, 
        int page, 
        int perPage
    )
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var productSelectConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? 
            throw new InvalidOperationException("Failed to open database connection.");

        var query = new StringBuilder(@"
            SELECT * 
            FROM product 
            WHERE 
                deleted_at IS NULL
            AND
                store_id = $1
        ");

        var parameters = new List<NpgsqlParameter>();
        parameters.Add(new() { Value = storeId });

        var index = 2;
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
        parameters.Add(new() { Value = (page - 1) * perPage < 0 ? 0 : (page - 1) * perPage });

        await using var command = new NpgsqlCommand(query.ToString(), productSelectConnection);
        command.Parameters.AddRange(parameters.ToArray());

        await using var productReader = command.ExecuteReader();

        var totalRecord = await CountAsync(storeId, productFilterDto);
        //var totalRecord = 100;

        if (productReader.HasRows)
        {
            var products = new List<Product>();

            while (await productReader.ReadAsync())
            {
                var product = productReaderToProduct(productReader);

                // SELECT product_option
                using var productOptionSelectConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

                await using var productOptionSelectCommand = new NpgsqlCommand(@"
                    SELECT *
                    FROM product_option
                    WHERE 
                        product_id = $1
                    AND
                        store_id = $2
                ", productOptionSelectConnection)
                {
                    Parameters =
                    {
                        new() { Value = product.Id },
                        new() { Value = product.StoreId }
                    }
                };

                await using var productOptionReader = productOptionSelectCommand.ExecuteReader();

                if (!productOptionReader.HasRows)
                {
                    // if the product has no option (which
                    // is not likely due to the creation
                    // pipeline), it does not get returned at all
                    continue;
                }

                var productOptions = new List<(string name, decimal price)>();

                while (await productOptionReader.ReadAsync())
                    productOptions.Add(productOptionReaderToProductOption(productOptionReader));

                product.ProductOptions = productOptions;

                products.Add(product);
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

    /// <summary>
    /// Updates an existing product by its ID.
    /// </summary>
    /// <param name="productDto">The product with updated information.</param>
    /// <returns>The updated product if found; otherwise, null.</returns>
    public async Task<Product?> UpdateByIdAsync(long storeId, Product productDto)
    {
        if (productDto.ProductOptions.Count == 0)
            throw new InvalidOperationException("Product must have at least one option.");

        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var productUpdateConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? 
            throw new InvalidOperationException("Failed to open database connection.");

        await using var productUpdateCommand = new NpgsqlCommand(@"
            UPDATE product
            SET 
                updated_by = $1,
                updated_at = $2,
                name = $3,
                description = $4,
                items_sold = $5, 
                category_store_id = $6, 
                category_id = $7,
                image_uris = $8,
                status = $9
            WHERE 
                store_id = $10
            AND
                id = $11
            RETURNING *
        ", productUpdateConnection)
        {
            Parameters =
            {
                new() { Value = productDto.UpdatedBy },
                new() { Value = productDto.UpdatedAt },
                new() { Value = productDto.Name },
                new() { Value = productDto.Description },
                new() { Value = productDto.ItemsSold },
                new() { Value = productDto.CategoryId != null ? storeId : null },
                new() { Value = productDto.CategoryId },
                new() { Value = productDto.ImageUris },
                new() { Value = productDto.Status },
                new() { Value = storeId },
                new() { Value = productDto.Id }
            }
        };

        await using var productReader = productUpdateCommand.ExecuteReader();

        if (!productReader.HasRows)
            return null;

        if (!await productReader.ReadAsync())
            return null;

        var product = productReaderToProduct(productReader);

        // "UPDATE" product_option
        // Actually, we delete all options and re-insert them
        using var productOptionDeleteConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ??
            throw new InvalidOperationException("Failed to open database connection.");

        await using var productOptionDeleteCommand = new NpgsqlCommand(@"
            DELETE FROM product_option
            WHERE 
                store_id = $1
            AND
                product_id = $2
        ", productOptionDeleteConnection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = product.Id }
            }
        };

        // for debug purposes only
        var rowsDeleted = await productOptionDeleteCommand.ExecuteNonQueryAsync();

        // INSERT INTO product_option
        var updatedProductOptions = new List<(string name, decimal price)>();

        foreach (var option in productDto.ProductOptions)
        {
            using var productOptionInsertConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

            await using var productOptionInsertCommand = new NpgsqlCommand(@"
                INSERT INTO product_option
                (
                    product_id,
                    store_id,
                    name,
                    price
                )
                VALUES 
                    ($1, $2, $3, $4)
                RETURNING *
            ", productOptionInsertConnection)
            {
                Parameters =
                {
                    new() { Value = product.Id },
                    new() { Value = product.StoreId },
                    new() { Value = option.name },
                    new() { Value = option.price }
                }
            };

            await using var productOptionReader = productOptionInsertCommand.ExecuteReader();
            await productOptionReader.ReadAsync();

            updatedProductOptions.Add(productOptionReaderToProductOption(productOptionReader));
        }

        product.ProductOptions = updatedProductOptions;

        return product;
    }

    /// <summary>
    /// Deletes a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <returns>The deleted product if found; otherwise, null.</returns>
    public async Task<Product?> DeleteByIdAsync(long storeId, long id, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? 
            throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE product
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

        // For soft delete operation, we actually don't need
        // to do anything about the product_option here

        return productReaderToProduct(reader);
    }

    /// <summary>
    /// Deletes multiple products by their IDs.
    /// </summary>
    /// <param name="ids">The list of IDs of the products to delete.</param>
    /// <returns>A list of deleted products.</returns>
    public async Task<List<Product>> DeleteByIdsAsync(long storeId, List<long> ids, Staff author)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        await using var command = new NpgsqlCommand(@"
            UPDATE product
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
            var products = new List<Product>();

            while (await reader.ReadAsync())
            {
                products.Add(productReaderToProduct(reader));
            }

            return products;
        }

        return new();
    }

    /// <summary>
    /// Counts the total number of products.
    /// </summary>
    /// <returns>The total number of products.</returns>
    public async Task<long> CountAsync(long storeId, ProductFilterDto productFilterDto)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var connection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection ?? throw new InvalidOperationException("Failed to open database connection.");

        var query = new StringBuilder(@"
            SELECT COUNT(*) 
            FROM product 
            WHERE 
                deleted_at IS NULL
            AND
                store_id = $1");

        var parameters = new List<NpgsqlParameter>();
        parameters.Add(new() { Value = storeId });

        var index = 2;
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

        return await command.ExecuteScalarAsync() as long? ?? 0;
    }

    private Product productReaderToProduct(NpgsqlDataReader reader)
    {
        Product product = new Product
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
        product.UpdatedBy = reader.IsDBNull(reader.GetOrdinal("updated_by")) ?
            null : reader.GetString(reader.GetOrdinal("updated_by"));
        product.UpdatedAt = reader.IsDBNull(reader.GetOrdinal("updated_at")) ?
            null : reader.GetDateTime(reader.GetOrdinal("updated_at"));
        product.DeletedBy = reader.IsDBNull(reader.GetOrdinal("deleted_by")) ?
            null : reader.GetString(reader.GetOrdinal("deleted_by"));
        product.DeletedAt = reader.IsDBNull(reader.GetOrdinal("deleted_at")) ?
            null : reader.GetDateTime(reader.GetOrdinal("deleted_at"));
        product.CategoryId = reader.IsDBNull(reader.GetOrdinal("category_id")) ? 
            null : reader.GetInt64(reader.GetOrdinal("category_id"));
        product.ImageUris = reader.IsDBNull(reader.GetOrdinal("image_uris"))  ?
            new List<string>() : new List<string>(reader.GetFieldValue<string[]>(reader.GetOrdinal("image_uris")));
        
        return product;
    }

    private (string name, decimal price) productOptionReaderToProductOption(NpgsqlDataReader productOptionReader)
    {
        var productOption = 
        (
            productOptionReader.GetString(productOptionReader.GetOrdinal("name")),
            productOptionReader.GetDecimal(productOptionReader.GetOrdinal("price"))
        );
        
        return productOption;
    }
}