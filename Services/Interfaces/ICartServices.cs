using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;

namespace MyWebApiApp.Services.Interfaces
{
    public interface ICartServices
    {
        IEnumerable<CartModel> GetAllCart(int? userId);
        // bool AddCart(int userId, List<CartItemDto> cartItems);

        int GetCartByUser(int? userId);
        bool DeleteCart(int cartId);
        bool UpdateTotal(int cartId);
    }
}