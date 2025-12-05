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
    internal class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public Task<bool> EmailExistsAsync(string email)
        {
            return _dbSet.AnyAsync(u => u.Email == email);
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return _dbSet.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetUserWithOrderAsync(int userId)
        {
            var user = await _dbSet.Include(o => o.Orders)
                                   .SingleOrDefaultAsync(u => u.Id == userId);

            if(user == null)
            {
                return null;
            }

            return user;
        }
    }
}
