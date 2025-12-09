using ECommerceAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Interfaces
{
    internal interface ICartService
    {

        Task<CartItemsResponseDto> AddToCartAsync(int userId, int productId, int quantity);
        Task<bool> RemoveFromCartAsync(int userId, int productId);
        Task<CartItemsResponseDto> UpdateQuantityAsync(int userId, int productId, int quantity);
        Task<IEnumerable<CartItemsResponseDto>> GetUserCartAsync(int userId);
        Task<CartItemsResponseDto> GetCartItemsAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
    }
}
