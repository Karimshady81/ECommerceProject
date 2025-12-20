using ECommerceAPI.Application.DTOs.Request;
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
    }
}
