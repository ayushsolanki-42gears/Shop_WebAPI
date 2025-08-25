using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Filters;
using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productService;

        public ProductController(IProductServices productServices)
        {
            _productService = productServices;
        }

        [HttpGet]
        #region Get All Products
        public IActionResult GetAllProducts()
        {
            var products = _productService.GetAllProducts();
            ApiResponse response;
            if (products == null || !products.Any())
            {
                response = new ApiResponse("Products Not Found", 404);
                return NotFound("Products not found");
            }
            response = new ApiResponse(products, "Products fetch successfully", 200);
            return Ok(response);
        }
        #endregion

        [LogAction("Product Insert")]
        [HttpPost]
        #region Add Product

        public IActionResult AddProduct([FromBody] CreateProductDto product)
        {
            ApiResponse response;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new ApiResponse(errors, "All Product Details is required", 400));
            }

            bool isInserted = _productService.AddProduct(product);
            if (!isInserted)
            {
                throw new Exception("Error while inserting Product");
            }

            response = new ApiResponse("Inserted Successfully", 200);
            return Ok(response);
        }
        #endregion

        [LogAction("Product Update")]
        [HttpPut("{ProductID}")]
        #region Update Product
        public IActionResult UpdateProduct(int productId, UpdateProductDto product)
        {
            ApiResponse response;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new ApiResponse(errors, "All product detail is required", 400));
            }

            if (productId <= 0 || productId != product.ProductID)
            {
                response = new ApiResponse("ProductID is required", 400);
                return BadRequest(response);
            }

            bool isUpdated = _productService.EditProduct(product);

            if (!isUpdated)
            {
                throw new Exception("Error while updating product");
            }

            response = new ApiResponse("Product Updated Successfully", 200);
            return Ok(response);
        }
        #endregion

        [LogAction("Product Delete")]
        [HttpPatch("Delete/{ProductID}")]
        #region Delete Product
        public IActionResult DeleteProduct(int productId)
        {
            ApiResponse response;

            if (productId <= 0)
            {
                throw new ArgumentException("ProductID is required or not valid");
            }

            bool isUpdated = _productService.DeleteProduct(productId);
            if (!isUpdated)
            {
                throw new Exception("Error while deleting product");
            }

            response = new ApiResponse("Product Delete Successfully", 200);
            return Ok(response);
        }
        #endregion
    }
}