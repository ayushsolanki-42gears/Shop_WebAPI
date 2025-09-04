using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceItemController : ControllerBase
    {
        private readonly IInvoiceItemServices _invoiceItemServices;
        public InvoiceItemController(IInvoiceItemServices invoiceItemServices)
        {
            _invoiceItemServices = invoiceItemServices;
        }

        [HttpGet("{invoiceId}")]
        public IActionResult GetAll(int invoiceId)
        {
            ApiResponse response;
            var invoiceItems = _invoiceItemServices.GetAllInvoiceItems(invoiceId);
            if (invoiceItems == null || !invoiceItems.Any())
            {
                response = new ApiResponse("Invoice Items not found", 400);
                return NotFound(response);
            }
            response = new ApiResponse(invoiceItems, "Invoice items fetch successfully", 200);
            return Ok(response);
        }
    }
}