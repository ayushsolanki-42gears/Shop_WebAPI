namespace MyWebApiApp.Models.DTOs
{
    public class InvoiceResponse
    {
        public int InvoiceID { get; set; }
        public int InvoiceItemCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}