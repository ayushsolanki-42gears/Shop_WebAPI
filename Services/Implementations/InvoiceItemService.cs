using MyWebApiApp.Data;
using MyWebApiApp.Models;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Services.Implementations
{
    public class InvoiceItemService : IInvoiceItemServices
    {
        private readonly InvoiceItemRepository _invoiceItemRepository;

        public InvoiceItemService(InvoiceItemRepository invoiceItemRepository)
        {
            _invoiceItemRepository = invoiceItemRepository;
        }
        public IEnumerable<InvoiceItemModel> GetAllInvoiceItems(int invoiceId)
        {
            var invoiceItems = _invoiceItemRepository.GetAll(invoiceId);
            return invoiceItems;
        }
    }
}