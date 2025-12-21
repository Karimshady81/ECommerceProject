using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Domain.Entities;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceAPI.IntegrationTests
{
    public class CartControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public CartControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task AddToCart_ReturnSuccess()
        {
            //Arrange
            var request = new AddToCartRequestDto
            {
                UserId = 1,
                ProductId = 1,
                Quantity = 2
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _client.PostAsync($"api/cart/add_to_cart",content);

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddToCart_WithNoProduct_ReturnBadRequest()
        {
            //Arrange
            var request = new AddToCartRequestDto
            {
                UserId = 1,
                ProductId = 20,
                Quantity = 2
            };

            var content = new StringContent(
                JsonSerializer.Serialize(request),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _client.PostAsync($"api/cart/add_to_cart", content);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RemoveFromCart_ReturnSuccess()
        {
            //Arrange
            var removeRequest = new RemoveFromCartRequestDto
            {
                UserId = 1,
                ProductId = 1
            };

            var content = new StringContent(
                JsonSerializer.Serialize(removeRequest),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _client.DeleteAsync($"api/cart/remove_from_cart?userId={removeRequest.UserId}&productId={removeRequest.ProductId}");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task RemoveFromCart_WithNoProduct_ReturnBadRequest()
        {
            //Arrange
            var removeRequest = new RemoveFromCartRequestDto
            {
                UserId = 1,
                ProductId = 20
            }; 

            var content = new StringContent(
                JsonSerializer.Serialize(removeRequest),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _client.DeleteAsync($"api/cart/remove_from_cart?userId={removeRequest.UserId}&productId={removeRequest.ProductId}");
            
            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task GetUserCart_ReturnSuccess()
        {
            //Arrange          

            //Act
            var response = await _client.GetAsync("api/cart/get_user_cart/1");

            //Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseString);
        }

        [Fact]
        public async Task GetUserCart_WithInvalidUser_ReturnsBadRequest()
        {
            //Act
            var response = await _client.GetAsync("api/cart/get_user_cart/999");

            //Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
