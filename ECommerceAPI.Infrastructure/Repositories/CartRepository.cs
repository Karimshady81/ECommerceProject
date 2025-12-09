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
    internal class CartRepository : Repository<CartItem>, ICartRepository
    {
        public CartRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CartItem>> GetUserCartAsync(int userId)
        {
            return await _dbSet.Where(u => u.UserId == userId)
                               .Include(p => p.Product)
                               .ToListAsync();
        }

        public async Task<CartItem?> GetCartItemsAsync(int userId, int productId)
        {
            return await _dbSet.Where(u => u.UserId == userId && u.ProductId == productId)
                               .Include(p => p.Product)    
                               .SingleOrDefaultAsync();
        }

        public async Task ClearUserCartAsync(int userId)
        {
            var cartItems = await _dbSet.Where(u => u.UserId == userId)
                                        .ToListAsync();

            _dbSet.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCartItemCountAsync(int userId)
        {
            return await _dbSet.Where(u => u.UserId == userId)
                               .CountAsync();
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int productId)
        {
            var cartItem = await _dbSet.Where(u => u.UserId == userId && u.ProductId == productId)
                                       .SingleOrDefaultAsync();

            if (cartItem == null)
                return false;

            _dbSet.Remove(cartItem);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
