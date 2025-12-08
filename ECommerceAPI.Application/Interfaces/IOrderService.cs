using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.DTOs.Response;
using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    internal interface IOrderService
    {
        Task<OrderResponseDto> CreateUserOrderAsync(CreateOrderRequestDto orderDto);
        Task<OrderResponseDto> GetOrderWithDetailsAsync(int orderId);
        Task<IEnumerable<OrderResponseDto>> GetOrdersByUser(int userId);
        Task<OrderResponseDto> GetOrderByNumber(string orderNumber);
        Task<IEnumerable<OrderResponseDto>> GetOrderByStatusAsync(OrderStatus status);
        Task<bool> DeleteOrderAsync(int orderId); 
    }
}
