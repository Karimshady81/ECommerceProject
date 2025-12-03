using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Interfaces
{
    internal interface ICategoryRepository : IRepository<Category> 
    {
        Task<IEnumerable<Category>> GetActiveCategoriesAsync(); // Navigation menu
        Task<Category> GetCategoryWithProductsAsync(int categoryId); // Category page
    }
}
