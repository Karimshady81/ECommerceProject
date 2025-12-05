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
    internal class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            return await _dbSet.Where(o => o.OrderId == orderId)
                               .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            return await _dbSet.Where(s => s.PaymentStatus == status)
                                .OrderByDescending(c => c.PaymentDate)
                               .Include(o => o.Order)
                               .ToListAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet.Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate)
                               .ToListAsync();
        }
    }
}
