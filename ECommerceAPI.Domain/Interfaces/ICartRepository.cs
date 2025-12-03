using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Interfaces
{
    internal interface ICartRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetUserCartAsync(int userId); // Cart page
        Task<CartItem> GetCartItemsAsync(int userId, int productId); // Check if already in cart
        Task ClearUserCartAsync(int userId); // After checkout
        Task<int> GetCartItemCountAsync(int userId);  // Cart badge/icon
    }
}
