using System.Data;
using MyWebApiApp.Models;
using Microsoft.Data.SqlClient;
using MyWebApiApp.Utilities;
using MyWebApiApp.Models.DTOs;
using MongoDB.Driver;

namespace MyWebApiApp.Data
{
    public class CartItemRepository
    {
        private readonly DBHelper _dBHelper;

        public CartItemRepository(DBHelper dBHelper)
        {
            _dBHelper = dBHelper;
        }

        #region Add Cart Item
        public bool AddCartItem(CartItemDto cartItemModel)
        {
            Console.WriteLine("Add Cart Item Repository");
            int rowAffected = _dBHelper.ExecuteNonQuery(
                "PR_CartItem_Add",
                new SqlParameter("@UserID", cartItemModel.UserID),
                new SqlParameter("@ProductID", cartItemModel.ProductID),
                new SqlParameter("@Quantity", cartItemModel.Quantity)
            );
            Console.WriteLine("After Add Cart Item Repository");

            return rowAffected > 0;
        }
        #endregion

        #region Update Cart Item
        public bool UpdateCartItem(int cartItemId, int quantity)
        {
            int rowAffected = _dBHelper.ExecuteNonQuery(
                "PR_CartItem_Update",
                new SqlParameter("@CartItemID", cartItemId),
                new SqlParameter("@Quantity", quantity)
            );
            return rowAffected > 0;
        }
        #endregion

        #region Delete Cart Item
        public bool DeleteCartItem(int cartItemId)
        {
            int rowAffected = _dBHelper.ExecuteNonQuery(
                "PR_CartItem_Delete",
                new SqlParameter("@CartItemID", cartItemId)
            );
            return rowAffected > 0;
        }
        #endregion

        #region Get Cart Items by Cart
        public IEnumerable<CartItemModel> GetCartItemsByCart(int? userId)
        {
            var items = new List<CartItemModel>();
            var dt = _dBHelper.ExecuteDataTable(
                "PR_CartItem_ListByCart",
                new SqlParameter("@UserID", userId)
            );

            foreach (DataRow row in dt.Rows)
            {
                items.Add(new CartItemModel()
                {
                    CartItemID = Convert.ToInt32(row["CartItemID"]),
                    ProductID = Convert.ToInt32(row["ProductID"]),
                    ProductName = row["ProductName"].ToString(),
                    Quantity = Convert.ToInt32(row["Quantity"]),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    Price = Convert.ToDecimal(row["Price"]),
                    Stock = Convert.ToInt32(row["Stock"])
                });
            }
            return items;
        }
        #endregion

        public CartItemResponse CheckForCart(int productId, int? userId)
        {
            CartItemResponse? item = null;
            var dt = _dBHelper.ExecuteDataTable(
                "PR_CartItem_GetIfExists",
                new SqlParameter("@UserID", userId),
                new SqlParameter("@ProductID", productId)
            );

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            var row = dt.Rows[0];
            Console.WriteLine("dfghjkl");
            item = new CartItemResponse()
            {
                CartItemID = row["CartItemID"] == DBNull.Value ? null : Convert.ToInt32(row["CartItemID"]),
                ProductID = row["ProductID"] == DBNull.Value ? 0 : Convert.ToInt32(row["ProductID"]),
                ProductName = row["ProductName"] == DBNull.Value ? string.Empty : row["ProductName"].ToString(),
                CartQuantity = row["CartQuantity"] == DBNull.Value ? 0 : Convert.ToInt32(row["CartQuantity"]),
                ProductStock = row["ProductStock"] == DBNull.Value ? 0 : Convert.ToInt32(row["ProductStock"]),
                TotalAmount = row["TotalAmount"] == DBNull.Value ? 0m : Convert.ToDecimal(row["TotalAmount"])
            };

            Console.WriteLine(item.ToString());
            Console.WriteLine(item.CartItemID);
            Console.WriteLine(item.CartQuantity);
            Console.WriteLine(item.ProductName);
            return item;
        }
    }
}
