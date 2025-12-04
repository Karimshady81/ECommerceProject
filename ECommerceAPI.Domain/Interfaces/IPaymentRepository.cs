using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<Payment> GetByOrderIdAsync(int orderId); // Verify order payments
        Task<IEnumerable<Payment>> GetPaymentsByStatusAsync(string status); // Admin: pending payments
        Task<IEnumerable<Payment>> GetPaymentsByDateRangeAsync(DateTime startDate, DateTime endDate); // Payment reports
    }
}
