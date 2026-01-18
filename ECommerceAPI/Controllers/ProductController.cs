using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("add_product")]
        public async Task<IActionResult> CreateProduct(CreateProductRequestDto product)
        {
            var createdProduct = await _productService.CreateProductAsync(product);
            return Ok(createdProduct);
        }

        [HttpGet("get_all_products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("get_product/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpGet("get_products_by_category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _productService.GetProductByCategoryAsync(categoryId);
            return Ok(products);
        }

        [HttpGet("get_active_products")]
        public async Task<IActionResult> GetActiveProducts()
        {
            var products = await _productService.GetActiveProductsAsync();
            return Ok(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("update_product/{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId,UpdateProductRequestDto updateProduct)
        {
            var updatedProduct = await _productService.UpdateProductAsync(productId, updateProduct);
            return Ok(updatedProduct);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("reduce_quantity/{productId}")]
        public async Task<IActionResult> ReduceStock(int productId,UpdateProductRequestDto reducedQuantity)
        {
            var updateQuantity = await _productService.ReduceStockAsync(productId, reducedQuantity);
            return Ok(updateQuantity);
        }

        [HttpGet("is_in_stock/{productId}")]
        public async Task<IActionResult> IsInStock(int productId, [FromQuery] int quantity)
        {
            var inStock = await _productService.IsInStockAsync(productId, quantity);

            return Ok(new
            {
                productId,
                requestedQuantity = quantity,
                inStock
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("delete_product/{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _productService.DeleteProductAsync(productId);
            return Ok(new
            {
                productId,
                message = "Product deleted"
            });
        }

    }
}
