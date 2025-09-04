using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;

namespace MyWebApiApp.Services.Interfaces
{
    public interface ICartItemServices
    {
        IEnumerable<CartItemModel> GetCartItemsByCart(int? userId);
        bool AddCartItem(CartItemDto cartItem);
        bool UpdateCartItem(int cartItemId, int Quantity);
        bool DeleteCartItem(int cartItemId);

        CartItemResponse CheckForCart(int productId,int? userId);
    }
}