namespace MyWebApiApp.Models.DTOs
{
    public class CartItemResponse
    {
        public int? CartItemID { get; set; }       // Nullable, because product may not exist in cart
        public int ProductID { get; set; }         // Always has ProductID
        public string ProductName { get; set; }    // Product name
        public int? CartQuantity { get; set; }     // Nullable, if product not in cart
        public int ProductStock { get; set; }      // Product stock
        public decimal? TotalAmount { get; set; }  // Nullable, if product not in cart
    }
}