using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Filters;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Implementations;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceServices _invoiceService;
        public InvoiceController(IInvoiceServices invoiceServices)
        {
            _invoiceService = invoiceServices;
        }

        #region Create Invoice from Cart
        [LogAction("Invoice Insert")]
        [HttpPost]
        public IActionResult CreateInvoiceFromCart()
        {
            ApiResponse response;
            int? userId = HttpContext.Session.GetInt32("UserID");
            bool isCreated = _invoiceService.InsertInvoice(userId);
            if (!isCreated)
            {
                throw new Exception("Error while creating invoice from cart");
            }

            response = new ApiResponse("Invoice created successfully", 200);
            return Ok(response);
        }
        #endregion

        #region List All Invoices
        [HttpGet]
        public IActionResult GetAllInvoices()
        {
            ApiResponse response;
            int? userId = null;
            string? role = HttpContext.Session.GetString("Role");
            if (role == "User")
            {
                userId = HttpContext.Session.GetInt32("UserID");
            }
            var invoices = _invoiceService.GetAllInvices(userId);
            if (invoices == null || !invoices.Any())
            {
                response = new ApiResponse("No invoices found", 404);
                return NotFound(response);
            }
            response = new ApiResponse(invoices, "Invoices Fetch Successfully", 200);
            return Ok(response);
        }
        #endregion

        [HttpGet("{invoiceId}")]
        #region Get Invoice By Id
        public IActionResult GetInvoiceById(int invoiceId)
        {
            ApiResponse response;
            if (invoiceId <= 0)
            {
                throw new ArgumentException("InvoiceId is invalid");
            }
            var invoice = _invoiceService.GetInvoiceById(invoiceId);
            if (invoice == null)
            {
                response = new ApiResponse("Invoice Not found", 404);
                return NotFound(response);
            }
            response = new ApiResponse(invoice, "Invoice Fetch successfully", 200);
            return Ok(response);
        }
        #endregion
    }
}
