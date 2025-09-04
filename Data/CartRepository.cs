using System.Data;
using Microsoft.Data.SqlClient;
using MyWebApiApp.Models;
using MyWebApiApp.Utilities;

namespace MyWebApiApp.Data
{
    public class CartRepository
    {
        private readonly DBHelper _dBHelper;

        public CartRepository(DBHelper dBHelper)
        {
            _dBHelper = dBHelper;
        }

        #region Add Cart
        public int AddCart(int userId, decimal totalAmount = 0.0m)
        {
            int cartId = Convert.ToInt32(_dBHelper.ExecuteScalar(
                "PR_Cart_Add",
                new SqlParameter("@UserID", userId),
                new SqlParameter("@TotalAmount", totalAmount)
            ));
            return cartId;
        }
        #endregion


        #region Get All Carts
        public IEnumerable<CartModel> SelectAllCarts(int? userId)
        {
            var carts = new List<CartModel>();
            var dt = _dBHelper.ExecuteDataTable(
                "PR_Cart_ListAll",
                new SqlParameter("@UserID", userId)
            );
            foreach (DataRow row in dt.Rows)
            {
                carts.Add(new CartModel()
                {
                    CartID = Convert.ToInt32(row["CartID"]),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    UserID = Convert.ToInt32(row["UserID"]),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                    CartItemCount = Convert.ToInt32(row["CartItemCount"])
                });
            }
            return carts;
        }
        #endregion

        #region Delete Cart
        public bool DeleteCart(int cartId)
        {
            int rowAffected = _dBHelper.ExecuteNonQuery(
                "PR_Cart_Delete",
                new SqlParameter("@CartID", cartId)
            );
            return rowAffected > 0;
        }
        #endregion

        #region Update Total Amount
        public bool UpdateCartTotal(int cartId)
        {
            int rowAffected = _dBHelper.ExecuteNonQuery(
                "PR_Cart_UpdateTotal",
                new SqlParameter("@CartID", cartId)
            );
            return rowAffected > 0;
        }
        #endregion
    }
}
