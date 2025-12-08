using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    internal interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllProducstAsync();
        Task<ProductResponseDto> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductResponseDto>> GetProductByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductResponseDto>> GetActiveProductsAsync();
        Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto productDto);
        Task<ProductResponseDto> UpdateProductAsync(int productId,UpdateProductRequestDto productDto);
        Task<bool> DeleteProductAsync(int productId);
        Task<ProductResponseDto> UpdateStockAsync(int productId, int quantity);
        Task<bool> IsInStock(int productId, int quantity);
    }
}
