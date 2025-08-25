using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;

namespace MyWebApiApp.Services.Interfaces
{
    public interface IProductServices
    {
        IEnumerable<ProductModel> GetAllProducts();
        bool AddProduct(CreateProductDto product);
        bool EditProduct(UpdateProductDto product);
        bool DeleteProduct(int productId); 
    }
}