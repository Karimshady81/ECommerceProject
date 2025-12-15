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
        public async Task<IActionResult> AddToCart(int userId, int productId, int quantity)
        {
            try
            {
                var addedToCart = await _cartService.AddToCartAsync(userId, productId, quantity);
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
    }
}
