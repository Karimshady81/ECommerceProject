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
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
        {
            return await _dbSet.Where(p => p.CategoryId == categoryId)
                               .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _dbSet.Where(p => p.IsActive)
                               .OrderBy(p => p.Name)
                               .ToListAsync();
        }

        public async Task<IEnumerable<Product>> SearchProductAsync(string searchTerm)
        {
            return await _dbSet.Where(p => p.Name.Contains(searchTerm))
                               .ToListAsync();
        }

        public async Task<bool> IsInStockAsync(int productId, int quantity)
        {
            return await _dbSet.AnyAsync(p => p.Id == productId && p.StockQuantity >= quantity);
        }

        public async Task ReduceStockAsync(int productId, int quantity)
        {
            var product = await _dbSet.FindAsync(productId);
            if (product == null)
            {
                throw new InvalidOperationException("Product not found.");
            }
            
            if (product.StockQuantity < quantity)
            {
                throw new InvalidOperationException("Insufficient stock.");
            }
            product.StockQuantity -= quantity;
            _dbSet.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
