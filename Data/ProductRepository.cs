using System.Data;
using Microsoft.Data.SqlClient;
using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Utilities;

namespace MyWebApiApp.Data
{
    public class ProductRepository
    {
        private readonly DBHelper _dBHelper;
        public ProductRepository(DBHelper dBHelper)
        {
            _dBHelper = dBHelper;
        }

        #region Get All Product
        public IEnumerable<ProductModel> SelectAllProducts()
        {
            var products = new List<ProductModel>();
            var dt = _dBHelper.ExecuteDataTable("PR_Product_ListAll");

            foreach (DataRow row in dt.Rows)
            {
                products.Add(new ProductModel()
                {
                    ProductID = row.Field<int>("ProductID"),
                    ProductName = row.Field<string>("ProductName") ?? string.Empty,
                    Price = row.Field<decimal>("Price"),
                    Quantity = row.Field<int>("Quantity"),
                    CreatedDate = row.Field<DateTime>("CreatedDate")
                });
            }
            return products;
        }
        #endregion

        #region Add Product
        public bool AddProduct(CreateProductDto product)
        {
            var parameters = new[]
            {
                new SqlParameter("@ProductName", product.ProductName),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@Quantity", product.Quantity)
            };
            return _dBHelper.ExecuteNonQuery("PR_Product_Add",parameters) > 0;
        }
        #endregion

        #region Update Product
        public bool UpdateProduct(UpdateProductDto product)
        {
            var parameters = new[]
            {
                new SqlParameter("@ProductName", product.ProductName),
                new SqlParameter("@Price", product.Price),
                new SqlParameter("@Quantity", product.Quantity),
                new SqlParameter("@ProductID", product.ProductID)
            };

            return _dBHelper.ExecuteNonQuery("PR_Product_Update", parameters) > 0;
        }
        #endregion

        #region Delete Product
        public bool DeleteProduct(int productId)
        {
            int rowAffected = _dBHelper.ExecuteNonQuery(
                "PR_Product_Delete",
                new SqlParameter("@ProductID", productId)
            );
            return rowAffected > 0;
        }
        #endregion
    }
}