namespace MyWebApiApp.Models
{
    public class InvoiceModel
    {
        public int InvoiceID { get; set; }
        public int UserID { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}