using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();
        Task<ProductResponseDto> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductResponseDto>> GetProductByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductResponseDto>> GetActiveProductsAsync();
        Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto productDto);
        Task<ProductResponseDto> UpdateProductAsync(int productId,UpdateProductRequestDto productDto);
        Task<bool> DeleteProductAsync(int productId);
        Task<ProductResponseDto> ReduceStockAsync(int productId, UpdateProductRequestDto reducedQuantity);
        Task<bool> IsInStockAsync(int productId, int quantity);
    }
}
