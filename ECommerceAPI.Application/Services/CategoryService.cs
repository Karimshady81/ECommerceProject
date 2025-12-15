using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.DTOs.Response;
using ECommerceAPI.Application.Interfaces;
using ECommerceAPI.Domain.Entities;
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

        public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryRequestDto categoryDto)
        {
            var categoryExists = await _categoryRepository.CategoryExistsAsync(categoryDto.Name);

            if (categoryExists)
                throw new InvalidOperationException($"Category '{categoryDto.Name}' already exists.");

            var category = new Category
            {              
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                IsActive = categoryDto.IsActive
            };

            var createdCategory = await _categoryRepository.AddAsync(category);

            return new CategoryResponseDto
            {
                Id = createdCategory.Id,
                Name = createdCategory.Name,
                Description = createdCategory.Description,
                IsActive = createdCategory.IsActive,
            };
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
                throw new InvalidOperationException($"No categories found with Id: {categoryId}");

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
