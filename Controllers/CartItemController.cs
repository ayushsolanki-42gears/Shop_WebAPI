using Microsoft.AspNetCore.Mvc;
using MyWebApiApp.Models;
using MyWebApiApp.Models.DTOs;
using MyWebApiApp.Services.Interfaces;

namespace MyWebApiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemServices _cartItemService;

        public CartItemController(ICartItemServices cartItemService)
        {
            _cartItemService = cartItemService;
        }

        #region List Cart Items
        [HttpGet]
        public IActionResult GetCartItems()
        {
            ApiResponse response;
            int? userId = HttpContext.Session.GetInt32("UserID");
            var items = _cartItemService.GetCartItemsByCart(userId);
            if (items == null || !items.Any())
            {
                response = new ApiResponse("Cart is Empty", 404);
                return NotFound(response);
            }
            response = new ApiResponse(items, "Cart Items fetch successfully", 200);
            return Ok(response);
        }
        #endregion

        #region Add Cart Item
        [HttpPost]
        public IActionResult AddCartItem([FromBody] CartItemDto cartItem)
        {
            ApiResponse response;
            if (cartItem == null || cartItem.ProductID <= 0)
            {
                response = new ApiResponse("Invalid cart item data", 400);
                return BadRequest(response);
            }
            int? userId = HttpContext.Session.GetInt32("UserID");
            cartItem.UserID = userId;
            bool isAdded = _cartItemService.AddCartItem(cartItem);
            if (!isAdded)
            {
                throw new Exception("Error while inserting cart item");
            }
            response = new ApiResponse("Cart item added successfully", 200);
            return Ok(response);
        }
        #endregion

        #region Update Cart Item
        [HttpPatch("{cartItemId}")]
        public IActionResult UpdateCartItem(int cartItemId, [FromBody] CartItemModel cartItem)
        {
            ApiResponse response;
            if (cartItemId <= 0 || cartItem.Quantity <= 0)
            {
                response = new ApiResponse("Invalid cart item data", 400);
                return BadRequest(response);
            }
            bool isUpdated = _cartItemService.UpdateCartItem(cartItemId, cartItem.Quantity);
            if (!isUpdated)
            {
                throw new Exception("Error while updating cart item");
            }
            response = new ApiResponse("Cart item updated successfully", 200);
            return Ok(response);
        }
        #endregion

        #region Delete Cart Item
        [HttpDelete("{cartItemId}")]
        public IActionResult DeleteCartItem(int cartItemId)
        {
            ApiResponse response;
            if (cartItemId <= 0)
            {
                throw new ArgumentException("CartItem ID is not valid.");
            }
            bool isDeleted = _cartItemService.DeleteCartItem(cartItemId);
            if (!isDeleted)
            {
                throw new Exception("Error while deleting cart item");
            }
            response = new ApiResponse("Cart item deleted successfully", 200);
            return Ok(response);
        }
        #endregion

        [HttpGet("CheckforCart/{productId}")]
        public IActionResult CheckForCart(int productId)
        {
            ApiResponse response;
            if (productId <= 0)
            {
                throw new ArgumentException("ProductId is not valid");
            }

            int? userId = HttpContext.Session.GetInt32("UserID");
            var data = _cartItemService.CheckForCart(productId, userId);
            if (data.CartItemID == null)
            {
                response = new ApiResponse(data, "Item not found in cart", 404);
                return Ok(response);
            }
            response = new ApiResponse(data, "Item Found in cart", 200);
            return Ok(response);
        }
    }
}
