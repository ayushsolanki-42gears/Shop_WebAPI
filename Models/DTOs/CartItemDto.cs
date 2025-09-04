namespace MyWebApiApp.Models.DTOs
{
    public class CartItemDto
    {
        public int? ProductID { get; set; }
        public int? Quantity { get; set; }
        public int? UserID { get; set; }
    }
}