using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.Application.DTOs.Response
{
    public class PaymentResponseDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string PaymentDate { get; set ; }
    }
}