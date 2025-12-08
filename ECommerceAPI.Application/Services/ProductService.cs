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
    internal class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAllProducstAsync()
        {
            var products = await _productRepository.GetAllAsync();

            if (products == null || !products.Any())
                throw new InvalidOperationException("No Products found");

            //prepare a list for the DTO
            var response = new List<ProductResponseDto>();

            //Map each product entity -> DTO
            foreach (var product in products)
            {
                response.Add(new ProductResponseDto
                {
                    Id = product.Id,
                    CategoryName = product.Category?.Name,
                    Name = product.Name,
                    Description = product.Description,
                    Image = product.Image,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    AddedDate = product.CreatedAt.ToString("B")
                });
            }

            return response;
        }

        public async Task<ProductResponseDto> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                throw new InvalidOperationException($"No product with this ID: {productId} ");

            return new ProductResponseDto
            {
                Id = product.Id,
                CategoryName = product.Category?.Name,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                AddedDate = product.CreatedAt.ToString("B")
            };
        }

        public async Task<IEnumerable<ProductResponseDto>> GetProductByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);

            if (products == null || !products.Any())
                throw new InvalidOperationException($"No products with this category Id: {categoryId}");

            var response = new List<ProductResponseDto>();

            foreach(var product in products)
            {
                response.Add(new ProductResponseDto
                {
                    Id = product.Id,
                    CategoryName = product.Category?.Name,
                    Name = product.Name,
                    Description = product.Description,
                    Image = product.Image,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    AddedDate = product.CreatedAt.ToString("D")
                });
            }

            return response;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetActiveProductsAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();

            if (products == null || !products.Any()) 
                throw new InvalidOperationException("No active products");

            var response = new List<ProductResponseDto>();

            foreach(var product in products)
            {
                response.Add(new ProductResponseDto
                {
                    Id = product.Id,
                    CategoryName = product.Category?.Name,
                    Name = product.Name,
                    Description = product.Description,
                    Image = product.Image,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    AddedDate = product.CreatedAt.ToString("B")
                });
            }

            return response;
        }

        public async Task<ProductResponseDto> CreateProductAsync(CreateProductRequestDto productDto)
        {
            var product = new Product
            {
                CategoryId = productDto.CategoryId,
                Name = productDto.Name,
                Description = productDto.Description,
                Image = productDto.Image,
                Price = productDto.Price,
                StockQuantity = productDto.StockQuantity,
                IsActive = productDto.IsActive,
                CreatedAt = DateTime.UtcNow,
            };

            var createdProduct = await _productRepository.AddAsync(product);

            return new ProductResponseDto
            {
                Id = createdProduct.Id,
                CategoryName = createdProduct.Category.Name,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Image = createdProduct.Image,
                Price = createdProduct.Price,
                StockQuantity = createdProduct.StockQuantity,
                AddedDate = createdProduct.CreatedAt.ToString("B")
            };
        }

        public async Task<ProductResponseDto> UpdateProductAsync(int productId,UpdateProductRequestDto productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productId);

            if (existingProduct == null)
                throw new InvalidOperationException($"No product found with this Id: {productId}");

            
            // Update only provided string fields
            if (!string.IsNullOrWhiteSpace(productDto.Name))
                existingProduct.Name = productDto.Name;

            if (!string.IsNullOrWhiteSpace(productDto.Description))
                existingProduct.Description = productDto.Description;

            if (!string.IsNullOrWhiteSpace(productDto.Image))
                existingProduct.Image = productDto.Image;

            // Update only provided numeric fields
            if (productDto.Price.HasValue)
                existingProduct.Price = productDto.Price.Value;

            if (productDto.StockQuantity.HasValue)
                existingProduct.StockQuantity = productDto.StockQuantity.Value;

            if (productDto.IsActive.HasValue)
                existingProduct.IsActive = productDto.IsActive.Value;

            existingProduct.UpdateAt = DateTime.UtcNow;

            //Update in database
            var updatedProduct = await _productRepository.UpdateAsync(existingProduct);

            return new ProductResponseDto
            {
                Id = existingProduct.Id,
                CategoryName = existingProduct.Category?.Name,
                Name = existingProduct.Name,
                Description = existingProduct.Description,
                Price = existingProduct.Price,
                StockQuantity = existingProduct.StockQuantity,
                UpdatedAt = existingProduct.UpdateAt.ToString("B")
            };
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var productExists = await _productRepository.GetByIdAsync(productId);
            if (productExists == null)
            {
                throw new InvalidOperationException($"No Product with this Id: {productId}");
            }

            return await _productRepository.DeleteAsync(productId);
        }

        public async Task<ProductResponseDto> UpdateStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new InvalidOperationException($"No product with this Id: ${productId}");
            }

            product.StockQuantity -= quantity;

            if (product.StockQuantity < 0) 
            {
                throw new InvalidOperationException("Not enough stock available");
            }

            var updated = await _productRepository.UpdateAsync(product);

            return new ProductResponseDto
            {
                Id = updated.Id,
                CategoryName = updated.Category?.Name,
                Name = updated.Name,
                Description = updated.Description,
                Image = updated.Image,
                Price = updated.Price,
                StockQuantity = updated.StockQuantity,
                UpdatedAt = updated.UpdateAt.ToString("B")
            };
        }

        public async Task<bool> IsInStockAsync(int productId, int quantity)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                throw new InvalidOperationException($"No product with this Id: {productId}");
            }

            return await _productRepository.IsInStockAsync(productId, quantity);
        }

    }
}
