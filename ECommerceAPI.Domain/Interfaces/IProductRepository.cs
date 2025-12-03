using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Interfaces
{
    internal interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(string categoryId); // Browse by category
        Task<IEnumerable<Product>> GetActiveProductsAsync(); // Show available products
        Task<IEnumerable<Product>> SearchProductAsync(string searchTerm); // Search bar
        Task<bool> IsInStockAsync(int productId, int quantity); // Before adding to cart
        Task UpdateStockAsync(int productId, int quantity); // After order placed
    }
}
