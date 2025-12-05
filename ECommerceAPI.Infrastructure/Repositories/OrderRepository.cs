using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Repositories
{
    internal class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetUserOrderAsync(int userId)
        {
            return await _dbSet.Where(u => u.UserId == userId).ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _dbSet.Where(o => o.Id == orderId)
                               .Include(oi => oi.OrderItems)
                               .ThenInclude(p => p.Product)
                               .FirstOrDefaultAsync();
        }

        public async Task<Order?> GetOrderByNumberAsync(string orderNumber)
        {
            return await _dbSet.SingleOrDefaultAsync(o => o.OrderNumber == orderNumber);
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _dbSet.Where(o => o.Status == status).ToListAsync();
        }

        public async Task<string> GenerateOrderNumberAsync()
        {
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";

            var exists = await _dbSet.AnyAsync(o => o.OrderNumber == orderNumber);
            if (exists)
            {
                return await GenerateOrderNumberAsync();
            }

            return orderNumber;
        }
    }
}
