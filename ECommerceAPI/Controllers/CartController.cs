using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("add_to_cart")]
        public async Task<IActionResult> AddToCart([FromBody]AddToCartRequestDto request)
        {
            var addedToCart = await _cartService.AddToCartAsync(request.UserId, request.ProductId, request.Quantity);
            return Ok(addedToCart);
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("remove_from_cart")]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            var removed = await _cartService.RemoveFromCartAsync(userId, productId);
            return Ok(new
            {
                UserId = userId,
                message = $"Removed product {productId} from cart successfully"
            });
        }

        [Authorize(Roles = "Customer")]
        [HttpPut("update_cart_quantity")]
        public async Task<IActionResult> UpdateCartQuantity(int userId,int productId, int quantity)
        {
            var updatedCart = await _cartService.UpdateQuantityAsync(userId, productId, quantity);
            return Ok(updatedCart);
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("get_user_cart/{userId}")]
        public async Task<IActionResult> GetUserCart(int userId)
        {
            var cart = await _cartService.GetUserCartAsync(userId);
            return Ok(cart);
        }

        [Authorize(Roles = "Customer")]
        [HttpDelete("clear_cart/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            await _cartService.ClearCartAsync(userId);
            return Ok(new
            {
                message = "cart cleared successfully"
            });
        }
    }
}
