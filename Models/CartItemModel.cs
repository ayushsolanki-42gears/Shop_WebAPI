namespace MyWebApiApp.Models
{
    public class CartItemModel
    {
        public int CartItemID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public int InvoiceID { get; set; }
        public int CartID { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }
    }
}