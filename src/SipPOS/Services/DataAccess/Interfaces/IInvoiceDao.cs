using SipPOS.DataTransfer.Entity;
using SipPOS.Models.Entity;

namespace SipPOS.Services.DataAccess.Interfaces;

public interface IInvoiceDao
{
    Task<long> GetNextInvoiceIdAsync(long storeId);
    Task<Invoice?> InsertAsync(long storeId, InvoiceDto invoiceDto);
    Task<long> GetTotalCountWithDateTimeFilterAsync
    (
        long storeId,
        DateOnly date,
        TimeOnly fromTime,
        TimeOnly toTime
    );
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