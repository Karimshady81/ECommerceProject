using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Application.Services;
using Moq;
using ECommerceAPI.Application.DTOs.Request;
using FluentAssertions;
using ECommerceAPI.Domain.Entities;

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
                Email = "test@mail.com",
                Password = "123456789@",
                ConfirmPassword = "123456789@",
                FirstName = "test",
                LastName = "test",
                PhoneNumber = ""
            };

            //Act
            Func<Task> act = async () => await _sut.RegisterUserAsync(user);

            //Assert
            await act.Should()
                     .ThrowAsync<InvalidOperationException>();            
        }

        [Fact]
        public async Task LoginUserAsync_houldThrowException_WhenEmailDoesntExist()
        {
            //Arrange
            var user = new LoginUserRequestDto
            {
                Email = "test1@mail.com",
                Password = "123456789@"
            };

            _userRepoMock
                .Setup(u => u.GetByEmailAsync(user.Email))
                .ReturnsAsync((User?)null);

            //Act
            Func<Task> act = async () => await _sut.LoginUserAsync(user);

            //Assert
            await act.Should()
                     .ThrowAsync<InvalidOperationException>()
                     .WithMessage("Invalid Credentials");
        }


    }
}
