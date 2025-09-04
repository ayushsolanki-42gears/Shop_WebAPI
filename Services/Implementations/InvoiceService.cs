using MyWebApiApp.Data;
using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Services.Implementations
{
    public class InvoiceService : IInvoiceServices
    {
        private readonly InvoiceRepository _invoiceRepository;
        public InvoiceService(InvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public bool InsertInvoice(int? userId)
        {
            if (userId == null) return false;
            bool inInserted = _invoiceRepository.CreateInvoiceFromCart(userId);
            return inInserted;
        }

        public IEnumerable<InvoiceResponse> GetAllInvices(int? userId)
        {
            var invoices = _invoiceRepository.GetAllInvoices(userId);
            return invoices;
        }

        public InvoiceResponse? GetInvoiceById(int invoiceId)
        {
            var invoice = _invoiceRepository.GetInvoiceByID(invoiceId);
            return invoice;
        }
    }
}