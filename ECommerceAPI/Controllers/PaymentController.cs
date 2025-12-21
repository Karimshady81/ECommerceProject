using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize]
        [HttpPost("create_payment")]
        public async Task<IActionResult> CreatePayment(int orderId, PaymentMethod paymentMethod)
        {
            try
            {
                var payment = await _paymentService.CreatePaymentAsync(orderId, paymentMethod);
                return Ok(payment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
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
        [HttpGet("get_payment_by_order")]
        public async Task<IActionResult> GetPaymentByOrderId(int orderId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByOrderIdAsync(orderId);
                return Ok(payment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
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
        [HttpGet("get_payment_by_status")]
        public async Task<IActionResult> GetPaymentByStatus(PaymentStatus status)
        {
            try
            {
                var payment = await _paymentService.GetPaymentsByStatusAsync(status);
                return Ok(payment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
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
        [HttpPut("update_payment_status")]
        public async Task<IActionResult> UpdatePaymentStatus(int paymentId, PaymentStatus status)
        {
            try
            {
                var updatePayment = await _paymentService.UpdatePaymentStatusAsync(paymentId,status);
                return Ok(updatePayment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
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
        [HttpGet("get_payment_by_date_range")]
        public async Task<IActionResult> GetPaymentsByDateRange(DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                var paymentRange = await _paymentService.GetPaymentByDateRangeAsync(dateStart, dateEnd);
                return Ok(paymentRange);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
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
