using System.ComponentModel.DataAnnotations;

namespace MyWebApiApp.Models.DTOs
{
    public class CreateProductDto
    {
        [Required(ErrorMessage = "Product Name is required")]
        [MaxLength(50)]
        public required string ProductName { set; get; }

        [Required(ErrorMessage = "Product price is required")]
        [Range(1, double.MaxValue)]
        public decimal Price { set; get; }

        [Required(ErrorMessage = "Product Quantity is required")]
        [Range(0,int.MaxValue)]
        public int Quantity { set; get; }
    }
}