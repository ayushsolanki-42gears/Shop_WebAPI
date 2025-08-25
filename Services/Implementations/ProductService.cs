using MyWebApiApp.Data;
using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Services.Implementations
{
    public class ProductService : IProductServices
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<ProductModel> GetAllProducts()
        {
            var products = _productRepository.SelectAllProducts();
            return products;
        }

        public bool AddProduct(CreateProductDto product)
        {
            bool isInserted = _productRepository.AddProduct(product);
            return isInserted;
        }

        public bool EditProduct(UpdateProductDto product)
        {
            bool isUpdated = _productRepository.UpdateProduct(product);
            return isUpdated;
        }

        public bool DeleteProduct(int productId)
        {
            bool isDeleted = _productRepository.DeleteProduct(productId);
            return isDeleted;
        }
    } 
}