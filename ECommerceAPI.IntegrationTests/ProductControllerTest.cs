using ECommerceAPI.Application.DTOs.Request;
using ECommerceAPI.Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceAPI.IntegrationTests
{
    public class ProductControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        //Constructor runs before each test
        public ProductControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProducts_ReturnsSuccessStatusCode()
        {
            //Arrange - nothing needed here

            //Act - make the HTTP request
            var response = await _client.GetAsync("api/products/get_all_products");

            //Assert - check the response
            response.EnsureSuccessStatusCode(); //status code 200-299

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseString);
        }

        [Fact]
        public async Task GetProducst_ReturnsProductsList()
        {
            //Act
            var response = await _client.GetAsync("api/products/get_all_products");

            //Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync(); //contains raw JSON text
            var products = JsonSerializer.Deserialize<List<ProductResponseDto>>(responseString); //"Deserialize" means: Convert JSON text → C# objects

            Assert.NotNull(products);
            Assert.NotEmpty(products); // Check we got products back
        }

        [Fact]
        public async Task CreateProduct_WithValidData_ReturnCreated()
        {
            //Arrange
            var newProduct = new CreateProductRequestDto
            {
                CategoryId = 1,
                Name = "test",
                Price = 1000,
                StockQuantity = 2,
                IsActive = true
            };

            var content = new StringContent(
                    JsonSerializer.Serialize(newProduct),
                    Encoding.UTF8,
                    "application/json");


            //Act
            var response = await _client.PostAsync("api/products/add_product", content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductById_NonExistingProduct_ReturnsNotFound()
        {
            //Act
            var respons = await _client.GetAsync("api/products/get_product/20000");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, respons.StatusCode);
        }
    }
}
