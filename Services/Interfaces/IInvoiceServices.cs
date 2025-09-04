using MyWebApiApp.Models.DTOs;

namespace MyWebApiApp.Services.Interfaces
{
    public interface IInvoiceServices
    {
        bool InsertInvoice(int? userId);
        IEnumerable<InvoiceResponse> GetAllInvices(int? userId);
        InvoiceResponse? GetInvoiceById(int invoiceId);      
    }
}