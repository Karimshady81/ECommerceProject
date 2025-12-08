using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Response
{
    internal class OrderResponseDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; }
        public decimal Total { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; }
        public List<OrderItemsResponseDto> OrderItems { get; set; }
        public string CreatedAt { get; set; }
    }
}
