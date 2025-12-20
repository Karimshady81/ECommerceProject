using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.Interfaces;
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

        [HttpPost("add_to_cart")]
        public async Task<IActionResult> AddToCart([FromBody]AddToCartRequestDto request)
        {
            try
            {
                var addedToCart = await _cartService.AddToCartAsync(request.UserId,request.ProductId,request.Quantity);
                return Ok(addedToCart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpDelete("remove_from_cart")]
        public async Task<IActionResult> RemoveFromCart(int userId, int productId)
        {
            try
            {
                var removed = await _cartService.RemoveFromCartAsync(userId, productId);
                return Ok(new
                {
                    UserId = userId,
                    message = $"Removed product {productId} from cart successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpPut("update_cart_quantity")]
        public async Task<IActionResult> UpdateCartQuantity(int userId,int productId, int quantity)
        {
            try
            {
                var updatedCart = await _cartService.UpdateQuantityAsync(userId, productId, quantity);
                return Ok(updatedCart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }

        [HttpGet("get_user_cart/{userId}")]
        public async Task<IActionResult> GetUserCart(int userId)
        {
            try
            {
                var cart = await _cartService.GetUserCartAsync(userId);
                return Ok(cart);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        } 

        [HttpDelete("clear_cart/{userId}")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            try
            {
                await _cartService.ClearCartAsync(userId);
                return Ok(new
                {
                    message = "cart cleared successfully"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                    inner = ex.InnerException?.Message
                });
            }
        }
    }
}
