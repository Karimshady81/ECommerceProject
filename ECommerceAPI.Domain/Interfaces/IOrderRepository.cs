using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Interfaces
{
    internal interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetUserOrderAsync(int userId); // Order history
        Task<Order> GetOrderWithDetailsAsync(int orderId); // Order details page
        Task<Order> GetOrderByNumberAsync(string orderNumber); // Track order
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status); // Admin filtering
        Task<string> GenerateOrderNumberAsync(); // Creating new order
    }
}
