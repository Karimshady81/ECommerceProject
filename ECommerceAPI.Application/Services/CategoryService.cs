using ECommerceAPI.Application.DTOs.Response;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetActiveCategoriesAsync()
        {
            var activeCategory = await _categoryRepository.GetActiveCategoriesAsync();

            if(!activeCategory.Any())
                return new List<CategoryResponseDto>();

            var response = new List<CategoryResponseDto>();

            foreach(var category in activeCategory)
            {
                response.Add(new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive,
                });
            }

            return response;
        }

        public async Task<CategoryResponseDto?> GetCategoryWithProductsAsync(int categoryId)
        {
            var categories = await _categoryRepository.GetCategoryWithProductsAsync(categoryId);

            if (categories == null)
                throw new InvalidOperationException("No categories found");

            return new CategoryResponseDto
            {
                Id = categories.Id,
                Name = categories.Name,
                Description = categories.Description,
                IsActive = categories.IsActive,
                Products = categories.Products.Select(p => new ProductResponseDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Image = p.Image,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    UpdatedAt = p.UpdateAt.ToString("D")
                }).ToList()
            };
        }
    }
}
