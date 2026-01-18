using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [Authorize(Roles = "Customer")]
        [HttpGet("get_order_by_order_id/{orderId}")]
        public async Task<IActionResult> GetOrderWithDetails (int orderId)
        {
            var order = await _orderService.GetOrderWithDetailsAsync(orderId);
            return Ok(order);
        }

        [Authorize(Roles = "Customer")]
        [HttpPost("check_out")]
        public async Task<IActionResult> CheckOut(int userId, string shippingAddress)
        {
            var checkOut = await _orderService.CheckoutAsync(userId, shippingAddress);
            return Ok(checkOut);
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("get_order_by_user")]
        public async Task<IActionResult> GetOrderByUser()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var order = await _orderService.GetOrdersByUser(userId);
            return Ok(order);
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("get_order_by_number/{orderNumber}")]
        public async Task<IActionResult> GetOrderByNumber(string orderNumber)
        {
            var order = await _orderService.GetOrderByNumber(orderNumber);
            return Ok(order);
        }

        [Authorize(Roles = "Customer,Admin")]
        [HttpGet("get_order_by_status")]
        public async Task<IActionResult> GetOrderByStatus(OrderStatus status)
        {
            var order = await _orderService.GetOrderByStatusAsync(status);
            return Ok(order);
        }
        
        [Authorize(Roles = "Customer")]
        [HttpDelete("delete_order/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var deletedOrder = await _orderService.DeleteOrderAsync(orderId);
            return Ok(new
            {
                Id = orderId,
                message = "Deleted order successfully"
            });
        }

    }
}
