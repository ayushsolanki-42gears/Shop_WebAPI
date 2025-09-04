using MyWebApiApp.Data;
using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Services.Implementations
{
    public class CartService : ICartServices
    {
        private readonly CartRepository _cartRepository;
        private readonly ICartItemServices _cartItemService;
        public CartService(CartRepository cartRepository, ICartItemServices cartItemServices)
        {
            _cartItemService = cartItemServices;
            _cartRepository = cartRepository;
        }

        public IEnumerable<CartModel> GetAllCart(int? userId)
        {
            var carts = _cartRepository.SelectAllCarts(userId);
            return carts;
        }

        // public bool AddCart(int userId, List<CartItemDto> cartItems)
        // {
        //     int cartId = _cartRepository.AddCart(userId);
        //     foreach (CartItemDto model in cartItems)
        //     {
        //         model.CartID = cartId;
        //         _cartItemService.AddCartItem(model);
        //     }
        //     UpdateTotal(cartId);
        //     return cartId > 0;
        // }

        public int GetCartByUser(int? userId)
        {
            return 1;
        }
        public bool DeleteCart(int cartId)
        {
            bool isDeleted = _cartRepository.DeleteCart(cartId);
            return isDeleted;
        }

        public bool UpdateTotal(int cartId)
        {
            bool isUpdated = _cartRepository.UpdateCartTotal(cartId);
            return isUpdated;
        }
    }
}