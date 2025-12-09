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
    internal class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IUserRepository userRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
        }

        public async Task<CartItemsResponseDto> AddToCartAsync(int userId, int productId, int quantity)
        {
            //Validate user
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("No user found");

            //Validate product
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException($"No product found with this Id: {productId} Or not enough quantity");

            //Check stock
            if (!await _productRepository.IsInStockAsync(productId, quantity))
                throw new InvalidOperationException($"Not enough quantity");

            //Check if product already in cart
            var existingCartItem = await _cartRepository.GetCartItemsAsync(userId,productId);

            if(existingCartItem != null)
            {
                // Update existing quantity
                existingCartItem.Quantity += quantity;
                await _cartRepository.UpdateAsync(existingCartItem);

                return new CartItemsResponseDto
                {
                    Id = existingCartItem.Id,
                    UserId = userId,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImage = product.Image,
                    UnitPrice = product.Price,
                    Quantity = existingCartItem.Quantity
                };
            }

            // Add new cart item
            var cart = new CartItem
            {
                UserId = user.Id,
                ProductId = product.Id,
                Quantity = quantity,
                CreatedAt = DateTime.UtcNow
            };

            var createdCart = await _cartRepository.AddAsync(cart);

            return new CartItemsResponseDto
            {
                UserId = createdCart.UserId,
                ProductId = createdCart.ProductId,
                ProductName = createdCart.Product.Name,
                ProductImage = createdCart.Product.Image,
                UnitPrice = createdCart.Product.Price,
                Quantity = quantity,
            };
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int productId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException("No user found");

            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new InvalidOperationException($"No product found with this Id: {productId} Or not enough quantity");

            return await _cartRepository.RemoveFromCartAsync(userId, productId);
        }

        public async Task<CartItemsResponseDto> UpdateQuantityAsync(int userId, int productId, int quantity)
        {
            // Validate user
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("No user found");

            // Validate product
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                throw new InvalidOperationException($"No product found with this Id: {productId}");

            // Get existing cart item
            var existingCartItem = await _cartRepository.GetCartItemsAsync(userId, productId);
            if (existingCartItem == null)
                throw new InvalidOperationException("This product does not exist in the cart.");

            // If quantity <= 0 → remove from cart
            if (quantity <= 0)
            {
                await _cartRepository.RemoveFromCartAsync(userId, productId);

                return new CartItemsResponseDto
                {
                    Id = existingCartItem.Id,
                    UserId = userId,
                    ProductId = productId,
                    ProductName = product.Name,
                    ProductImage = product.Image,
                    UnitPrice = product.Price,
                    Quantity = 0
                };
            }

            // Check stock for the NEW quantity
            if (!await _productRepository.IsInStockAsync(productId, quantity))
                throw new InvalidOperationException("Not enough stock for this quantity");

            // Update quantity
            existingCartItem.Quantity = quantity;
            await _cartRepository.UpdateAsync(existingCartItem);

            // Return updated cart item
            return new CartItemsResponseDto
            {
                Id = existingCartItem.Id,
                UserId = userId,
                ProductId = product.Id,
                ProductName = product.Name,
                ProductImage = product.Image,
                UnitPrice = product.Price,
                Quantity = existingCartItem.Quantity
            };
        }




    }
}
