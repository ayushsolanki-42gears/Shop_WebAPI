namespace MyWebApiApp.Models
{
    public class InvoiceItemModel
    {
        public int InvoiceItemID { get; set; }
        public int InvoiceID { get; set; }
        public int ProductID { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
    }
}