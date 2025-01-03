using SipPOS.DataTransfer.Entity;
using SipPOS.Models.Entity;

namespace SipPOS.Services.DataAccess.Interfaces;

public interface IInvoiceDao
{
    Task<Invoice?> InsertAsync(long storeId, InvoiceDto invoiceDto);

    Task<long> GetNextInvoiceIdAsync(long storeId);

    // For invoices, the most important thing is to store
    // and retrieve them, so Update and Delete are generally
    // not needed.

    //Task<List<Invoice>?> GetAllAsync();
    //Task<List<Invoice>?> GetWithPaginationAsync();
}