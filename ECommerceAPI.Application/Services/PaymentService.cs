using ECommerceAPI.Application.DTOs.Response;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Services
{
    internal class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IPaymentRepository paymentRepository, IOrderRepository orderRepository)
        {
            _paymentRepository = paymentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<PaymentResponseDto> CreatePaymentAsync(int orderId, PaymentMethod paymentMethod)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                throw new InvalidOperationException("Order not found");

            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = order.Total,
                PaymentMethod = paymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                PaymentDate = DateTime.UtcNow
            };

            var savedPayment = await _paymentRepository.AddAsync(payment);

            order.Status = OrderStatus.Paid;
            await _orderRepository.UpdateAsync(order);

            return new PaymentResponseDto
            {
                Id = savedPayment.Id,
                OrderId = savedPayment.OrderId,
                PaymentMethod = savedPayment.PaymentMethod,
                Amount = savedPayment.Amount,
                PaymentStatus = savedPayment.PaymentStatus,
                PaymentDate = savedPayment.PaymentDate.ToString("D")
            };
        }

        public async Task<PaymentResponseDto> GetPaymentByOrderIdAsync(int orderId)
        {
            var payment = await _paymentRepository.GetPaymentByOrderIdAsync(orderId);

            if (payment == null)
                throw new InvalidOperationException("No payments for this order");

            return new PaymentResponseDto
            {
                Id = payment.Id,
                OrderId = payment.OrderId,
                PaymentMethod = payment.PaymentMethod,
                Amount = payment.Amount,
                PaymentStatus = payment.PaymentStatus,
                PaymentDate = payment.PaymentDate.ToString("D")
            };
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            var payments = await _paymentRepository.GetPaymentsByStatusAsync(status);

            if (!payments.Any())
                return new List<PaymentResponseDto>();

            var response = new List<PaymentResponseDto>();

            foreach(var payment in payments)
            {
                response.Add(new PaymentResponseDto
                {
                    Id = payment.Id,
                    OrderId = payment.OrderId,
                    PaymentMethod = payment.PaymentMethod,
                    Amount = payment.Amount,
                    PaymentStatus = payment.PaymentStatus,
                    PaymentDate = payment.PaymentDate.ToString("D")
                });
            }

            return response;
        }

        public async Task<PaymentResponseDto> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);

            if (payment == null)
                throw new InvalidOperationException("No payment found");

            payment.PaymentStatus = status;
            payment.PaymentDate = DateTime.UtcNow;

            var updatedpayment = await _paymentRepository.UpdateAsync(payment);

            var order = await _orderRepository.GetByIdAsync(payment.OrderId);

            if (order != null)
            {
                switch (status)
                {
                    case PaymentStatus.Pending:
                        order.Status = OrderStatus.Pending;
                        break;

                    case PaymentStatus.Completed:
                        order.Status = OrderStatus.Paid;
                        break;

                    case PaymentStatus.Failed:
                        order.Status = OrderStatus.Cancelled;
                        break;
                }

                await _orderRepository.UpdateAsync(order);
            }

            return new PaymentResponseDto
            {
                Id = updatedpayment.Id,
                OrderId = updatedpayment.OrderId,
                Amount = updatedpayment.Amount,
                PaymentMethod = updatedpayment.PaymentMethod,
                PaymentStatus = updatedpayment.PaymentStatus,
                PaymentDate = updatedpayment.PaymentDate.ToString("D")
            };
        }

        public async Task<IEnumerable<PaymentResponseDto>> GetPaymentByDateRangeAsync(DateTime dateStart, DateTime dateEnd)
        {
            var payments = await _paymentRepository.GetPaymentsByDateRangeAsync(dateStart,dateEnd);

            if(!payments.Any())
                throw new InvalidOperationException(
                            $"No payments found between {dateStart:yyyy-MM-dd} and {dateEnd:yyyy-MM-dd}");

            var response = new List<PaymentResponseDto>();

            foreach(var pay in payments)
            {
                response.Add(new PaymentResponseDto
                {
                    Id = pay.Id,
                    OrderId = pay.OrderId,
                    PaymentMethod = pay.PaymentMethod,
                    Amount = pay.Amount,
                    PaymentStatus = pay.PaymentStatus,
                    PaymentDate = pay.PaymentDate.ToString("D")
                });
            }

            return response;
        }
    }
}
