using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("get_order_by_order_id/{orderId}")]
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

        [Authorize]
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

        [Authorize]
        [HttpGet("get_order_by_user/{userId}")]
        public async Task<IActionResult> GetOrderByUser(int userId)
        {
            try
            {
                var order = await _orderService.GetOrdersByUser(userId);
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

        [Authorize]
        [HttpGet("get_order_by_number/{orderNumber}")]
        public async Task<IActionResult> GetOrderByNumber(string orderNumber)
        {
            try
            {
                var order = await _orderService.GetOrderByNumber(orderNumber);
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

        [Authorize]
        [HttpGet("get_order_by_status")]
        public async Task<IActionResult> GetOrderByStatus(OrderStatus status)
        {
            try
            {
                var order = await _orderService.GetOrderByStatusAsync(status);
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
        
        [Authorize]
        [HttpDelete("delete_order/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            try
            {
                var deletedOrder = await _orderService.DeleteOrderAsync(orderId);
                return Ok(new
                {
                    Id = orderId,
                    message = "Deleted order successfully"
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
