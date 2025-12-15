using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("get_order/{orderId}")]
        public async Task<IActionResult> GetOrderWithDetails (int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderWithDetailsAsync(orderId);
                return Ok(order);
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

        [HttpPost("check_out")]
        public async Task<IActionResult> CheckOut(int userId, string shippingAddress)
        {
            try
            {
                var checkOut = await _orderService.CheckoutAsync(userId, shippingAddress);
                return Ok(checkOut);
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
