using MyWebApiApp.Models;

namespace MyWebApiApp.Services.Interfaces
{
    public interface IInvoiceItemServices
    {
        IEnumerable<InvoiceItemModel> GetAllInvoiceItems(int invoiceId);
    }
}