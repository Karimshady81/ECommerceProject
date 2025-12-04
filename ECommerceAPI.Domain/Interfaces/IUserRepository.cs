using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email); // Login
        Task<bool> EmailExistsAsync(string email); // Registration validation
        Task<User> GetUserWithOrderAsync(int userId); // User profile/history
    }
}
