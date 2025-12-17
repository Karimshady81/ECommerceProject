using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Application.Services;
using Moq;
using ECommerceAPI.Application.DTOs.Request;
using FluentAssertions;

namespace ECommerceAPI.Application.Tests
{
    public class AuthServiceTest
    {
        private readonly Mock<IUserRepository> _userRepoMock;

        private readonly AuthService _sut;

        public AuthServiceTest()
        {
            _userRepoMock = new Mock<IUserRepository>();

            _sut = new AuthService(_userRepoMock.Object);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowException_WhenFieldsAreMissing()
        {
            //Arrange
            var user = new RegisterUserRequestDto
            {
                Email = "",
                Password = "123456789@",
                ConfirmPassword = "123456789@",
                FirstName = "test",
                LastName = "test",
                PhoneNumber = "0123456789"
            };

            //Act
            Func<Task> act = async () => await _sut.RegisterUserAsync(user);

            //Assert
            await act.Should()
                     .ThrowAsync<InvalidOperationException>();            
        }
    }
}
