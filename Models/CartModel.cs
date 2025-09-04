namespace MyWebApiApp.Models
{
    public class CartModel
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CartItemCount { get; set; }
    }
}