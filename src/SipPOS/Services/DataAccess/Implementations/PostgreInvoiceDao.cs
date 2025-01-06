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
using Windows.UI.Xaml.Documents;

namespace SipPOS.Services.DataAccess.Implementations;

public class PostgreInvoiceDao : IInvoiceDao
{
    public async Task<long> GetNextInvoiceIdAsync(long storeId)
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var selectInvoiceCountConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var selectInvoiceCountCommand = new NpgsqlCommand(@"
            SELECT COUNT(*)
            FROM invoice
            WHERE store_id = $1
        ", selectInvoiceCountConnection)
        {
            Parameters =
            {
                new() { Value = storeId },
            }
        };

        var selectInvoiceCountResult = await selectInvoiceCountCommand.ExecuteScalarAsync();

        if (selectInvoiceCountResult == null)
            throw new InvalidOperationException("Failed to get next invoice id.");

        return (long)selectInvoiceCountResult;
    }

    public async Task<Invoice?> InsertAsync(long storeId, InvoiceDto invoiceDto)
    {
        if (invoiceDto.InvoiceItems.Count == 0)
            throw new InvalidOperationException("Invoice must have at least one item.");

        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        // INSERT INTO invoice
        using var invoiceInsertConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var invoiceInsertCommand = new NpgsqlCommand(@"
            INSERT INTO invoice 
            (
                store_id,
                staff_id,
                staff_name,
                created_at,
                item_count,
                sub_total,
                total_discount,
                invoice_based_vat,
                total,
                customer_paid,
                change,
                coupon_code,
                payment_method
            )
            VALUES 
                ($1, $2, $3, $4, $5, $6, $7, $8, $9, $10, $11, $12, $13)
            RETURNING *
        ", invoiceInsertConnection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = invoiceDto.StaffId },
                new() { Value = invoiceDto.StaffName },
                new() { Value = invoiceDto.CreatedAt },
                new() { Value = invoiceDto.ItemCount },
                new() { Value = invoiceDto.SubTotal },
                new() { Value = invoiceDto.TotalDiscount },
                new() { Value = invoiceDto.InvoiceBasedVAT },
                new() { Value = invoiceDto.Total },
                new() { Value = invoiceDto.CustomerPaid },
                new() { Value = invoiceDto.Change },
                new() { Value = invoiceDto.CouponCode },
                new() { Value = invoiceDto.PaymentMethod }
            }
        };

        await using var invoiceReader = invoiceInsertCommand.ExecuteReader();

        // Insert had failed
        if (!invoiceReader.HasRows)
            return null;

        // Insert successful
        await invoiceReader.ReadAsync();
        var resultInvoiceDto = invoiceReaderToInvoiceDto(invoiceReader);

        // INSERT INTO invoice_item
        foreach (var item in invoiceDto.InvoiceItems)
        {
            using var invoiceItemInsertConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

            await using var invoiceItemInsertCommand = new NpgsqlCommand(@"
                INSERT INTO invoice_item
                (
                    invoice_id,
                    store_id,
                    product_id,
                    item_name,
                    option_name,
                    option_price,
                    discount,
                    note
                )
                VALUES 
                    ($1, $2, $3, $4, $5, $6, $7, $8)
                RETURNING *
            ", invoiceItemInsertConnection)
            {
                Parameters =
                {
                    new() { Value = resultInvoiceDto.Id },
                    new() { Value = storeId },
                    new() { Value = item.ProductId },
                    new() { Value = item.ItemName },
                    new() { Value = item.OptionName },
                    new() { Value = item.OptionPrice },
                    new() { Value = item.Discount },
                    new() { Value = item.Note }
                }
            };

            await using var invoiceItemReader = invoiceItemInsertCommand.ExecuteReader();

            if (!invoiceItemReader.HasRows)
                throw new InvalidOperationException("Failed to insert invoice item.");

            await invoiceItemReader.ReadAsync();

            var ordinal = resultInvoiceDto.InvoiceItems.Count;

            resultInvoiceDto.InvoiceItems.Add(new InvoiceItemDto()
            {
                Id = invoiceItemReader.GetInt64(invoiceItemReader.GetOrdinal("id")),
                InvoiceId = invoiceItemReader.GetInt64(invoiceItemReader.GetOrdinal("invoice_id")),
                ProductId = invoiceItemReader.GetInt64(invoiceItemReader.GetOrdinal("product_id")),
                Ordinal = ordinal,
                ItemName = invoiceItemReader.GetString(invoiceItemReader.GetOrdinal("item_name")),
                OptionName = invoiceItemReader.GetString(invoiceItemReader.GetOrdinal("option_name")),
                OptionPrice = invoiceItemReader.GetDecimal(invoiceItemReader.GetOrdinal("option_price")),
                Discount = invoiceItemReader.GetDecimal(invoiceItemReader.GetOrdinal("discount")),
                Note = invoiceItemReader.GetString(invoiceItemReader.GetOrdinal("note")),
            });
        }

        return new Invoice(resultInvoiceDto);
    }

    public async Task<long> GetTotalCountWithDateTimeFilterAsync
    (
        long storeId,
        DateOnly date,
        TimeOnly fromTime,
        TimeOnly toTime
    )
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var countConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        await using var getCountCommand = new NpgsqlCommand(@"
            SELECT COUNT(*)
            FROM invoice
            WHERE 
                store_id = $1
            AND
                created_at BETWEEN $2 AND $3
        ", countConnection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = new DateTime(date, fromTime) },
                new() { Value = new DateTime(date, toTime) }
            }
        };

        var countScalar = await getCountCommand.ExecuteScalarAsync();
        var count = countScalar == null ? 0 : (long)countScalar;

        return count;
    }

    public async Task<List<Invoice>?> GetWithPaginationAsync
    (
        long storeId,
        long page,
        long rowsPerPage, 
        DateOnly date, 
        TimeOnly fromTime, 
        TimeOnly toTime
    )
    {
        var databaseConnectionService = App.GetService<IDatabaseConnectionService>();

        using var selectInvoiceConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

        var take = rowsPerPage;
        var skip = (page - 1) * rowsPerPage;

        await using var selectInvoiceCommand = new NpgsqlCommand(@"
            SELECT *
            FROM invoice
            WHERE 
                store_id = $1
            AND
                created_at BETWEEN $2 AND $3
            ORDER BY created_at DESC
            LIMIT $4
            OFFSET $5
        ", selectInvoiceConnection)
        {
            Parameters =
            {
                new() { Value = storeId },
                new() { Value = new DateTime(date, fromTime) },
                new() { Value = new DateTime(date, toTime) },
                new() { Value = take },
                new() { Value = skip }
            }
        };

        await using var invoiceReader = selectInvoiceCommand.ExecuteReader();

        if (!invoiceReader.HasRows)
            return null;

        var invoices = new List<Invoice>();

        while (await invoiceReader.ReadAsync())
        {
            var invoiceDto = invoiceReaderToInvoiceDto(invoiceReader);

            // read invoice items
            var selectInvoiceItemConnection = databaseConnectionService.GetOpenConnection() as NpgsqlConnection;

            await using var selectInvoiceItemCommand = new NpgsqlCommand(@"
                SELECT *
                FROM invoice_item
                WHERE 
                    store_id = $1
                AND
                    invoice_id = $2
            ", selectInvoiceItemConnection)
                {
                    Parameters =
                {
                    new() { Value = storeId },
                    new() { Value = invoiceDto.Id }
                }
            };

            await using var invoiceItemReader = selectInvoiceItemCommand.ExecuteReader();

            if (!invoiceItemReader.HasRows)
                continue;

            while (await invoiceItemReader.ReadAsync())
            {
                var invoiceItemDto = new InvoiceItemDto
                {
                    Id = invoiceItemReader.GetInt64(invoiceItemReader.GetOrdinal("id")),
                    InvoiceId = invoiceItemReader.GetInt64(invoiceItemReader.GetOrdinal("invoice_id")),
                    ProductId = invoiceItemReader.GetInt64(invoiceItemReader.GetOrdinal("product_id")),
                    ItemName = invoiceItemReader.GetString(invoiceItemReader.GetOrdinal("item_name")),
                    OptionName = invoiceItemReader.GetString(invoiceItemReader.GetOrdinal("option_name")),
                    OptionPrice = invoiceItemReader.GetDecimal(invoiceItemReader.GetOrdinal("option_price")),
                    Discount = invoiceItemReader.GetDecimal(invoiceItemReader.GetOrdinal("discount")),
                    Note = invoiceItemReader.GetString(invoiceItemReader.GetOrdinal("note"))
                };

                invoiceItemDto.Ordinal = invoiceItemDto.Id + 1;

                invoiceDto.InvoiceItems.Add(invoiceItemDto);
            }

            invoices.Add(new Invoice(invoiceDto));
        }

        return invoices;
    }

    private InvoiceDto invoiceReaderToInvoiceDto(NpgsqlDataReader reader)
    {
        var invoiceDto = new InvoiceDto
        {
            Id = reader.GetInt64(reader.GetOrdinal("id")),
            StaffId = reader.GetInt64(reader.GetOrdinal("staff_id")),
            StaffName = reader.GetString(reader.GetOrdinal("staff_name")),
            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
            ItemCount = reader.GetInt64(reader.GetOrdinal("item_count")),
            SubTotal = reader.GetDecimal(reader.GetOrdinal("sub_total")),
            TotalDiscount = reader.GetDecimal(reader.GetOrdinal("total_discount")),
            InvoiceBasedVAT = reader.GetDecimal(reader.GetOrdinal("invoice_based_vat")),
            Total = reader.GetDecimal(reader.GetOrdinal("total")),
            CustomerPaid = reader.GetDecimal(reader.GetOrdinal("customer_paid")),
            Change = reader.GetDecimal(reader.GetOrdinal("change")),
            CouponCode = reader.GetString(reader.GetOrdinal("coupon_code")),
            PaymentMethod = reader.GetString(reader.GetOrdinal("payment_method"))
        };

        return invoiceDto;
    }
}