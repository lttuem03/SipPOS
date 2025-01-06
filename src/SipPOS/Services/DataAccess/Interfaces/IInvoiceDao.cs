using SipPOS.DataTransfer.Entity;
using SipPOS.Models.Entity;

namespace SipPOS.Services.DataAccess.Interfaces;

/// <summary>
/// Interface for managing invoice data access operations.
/// </summary>
public interface IInvoiceDao
{
    /// <summary>
    /// Retrieves the next invoice ID for a given store.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <returns>The next invoice ID.</returns>
    Task<long> GetNextInvoiceIdAsync(long storeId);

    /// <summary>
    /// Inserts a new invoice into the database.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="invoiceDto">The invoice data transfer object to insert.</param>
    /// <returns>The inserted invoice, or null if the insertion failed.</returns>
    Task<Invoice?> InsertAsync(long storeId, InvoiceDto invoiceDto);

    /// <summary>
    /// Retrieves the total count of invoices with a date and time filter.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="date">The date to filter by.</param>
    /// <param name="fromTime">The start time to filter by.</param>
    /// <param name="toTime">The end time to filter by.</param>
    /// <returns>The total count of invoices that match the filter criteria.</returns>
    Task<long> GetTotalCountWithDateTimeFilterAsync
    (
        long storeId,
        DateOnly date,
        TimeOnly fromTime,
        TimeOnly toTime
    );

    /// <summary>
    /// Retrieves invoices with pagination and a date and time filter.
    /// </summary>
    /// <param name="storeId">The ID of the store.</param>
    /// <param name="page">The page number.</param>
    /// <param name="rowsPerPage">The number of rows per page.</param>
    /// <param name="date">The date to filter by.</param>
    /// <param name="fromTime">The start time to filter by.</param>
    /// <param name="toTime">The end time to filter by.</param>
    /// <returns>A list of invoices that match the filter criteria.</returns>
    Task<List<Invoice>?> GetWithPaginationAsync
    (
        long storeId, 
        long page,
        long rowsPerPage,
        DateOnly date, 
        TimeOnly fromTime, 
        TimeOnly toTime
    );

    // For invoices, the most important thing is to store
    // and retrieve them, so Update and Delete are generally
    // not needed.
}