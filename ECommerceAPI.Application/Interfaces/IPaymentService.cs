using ECommerceAPI.Application.DTOs.Response;
using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreatePaymentAsync(int orderId, PaymentMethod paymentMethod);
        Task<PaymentResponseDto> GetPaymentByOrderIdAsync(int orderId);
        Task<IEnumerable<PaymentResponseDto>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<PaymentResponseDto> UpdatePaymentStatusAsync(int paymentId, PaymentStatus status);
        Task<IEnumerable<PaymentResponseDto>> GetPaymentByDateRangeAsync(DateTime dateStart, DateTime dateEnd);
    }
}
