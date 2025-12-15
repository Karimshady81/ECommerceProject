using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryRequestDto categoryDto);
        Task<IEnumerable<CategoryResponseDto>> GetActiveCategoriesAsync();
        Task<CategoryResponseDto?> GetCategoryWithProductsAsync(int categoryId);
    }
}
