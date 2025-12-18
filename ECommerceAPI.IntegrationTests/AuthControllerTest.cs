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
    public class AuthControllerTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory _factory;

        public AuthControllerTest(CustomWebApplicationFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegisterUser_WithValidData_ReturnsCreated()
        {
            //Arrange 
            var user = new RegisterUserRequestDto
            {
                Email = "test@mail.com",
                Password = "password",
                ConfirmPassword = "password",
                FirstName = "test",
                LastName = "test",
                PhoneNumber = "123456789",
            };

            var content = new StringContent(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8,
                    "application/json");

            //Act
            var response = await _client.PostAsync("api/users/register", content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task RegisterUser_WithInValidData_ReturnsBadRequest()
        {
            //Arrange
            var user = new RegisterUserRequestDto
            {
                Email = "test@mail.com",
                Password = "password",
                ConfirmPassword = "password",
                FirstName = "test",
                LastName = "test",
                PhoneNumber = "",
            };

            var content = new StringContent(
                JsonSerializer.Serialize(user),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _client.PostAsync("api/users/register", content);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task LoginUser_WithValidData_ShouldReturnOK()
        {
            //Arrange
            var loginData = new LoginUserRequestDto
            {
                Email = "Test@mail.com",
                Password = "123456789"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginData),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _client.PostAsync("api/users/login", content);

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task LoginUser_WithInValidData_ShouldReturnBadRequest()
        {
            //Arrange
            var loginData = new LoginUserRequestDto
            {
                Email = "Invalid@mail.com",
                Password = "123456789"
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginData),
                Encoding.UTF8,
                "application/json");

            //Act
            var response = await _client.PostAsync("api/users/login", content);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetUserProfile_ShouldReturnProfile()
        {
            //Act
            var response = await _client.GetAsync("api/users/profile/1");

            //Assert
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Raw JSON Response: {responseString}");  // ADD THIS LINE!

            var user = JsonSerializer.Deserialize<AuthResponseDto>(responseString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            Assert.NotNull(user);
            Assert.Equal("Test@mail.com", user.Email);
            Assert.Equal("test", user.FirstName);
        }

    }
}
