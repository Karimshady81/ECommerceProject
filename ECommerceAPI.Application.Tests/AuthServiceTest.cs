using ECommerceAPI.Domain.Interfaces;
using ECommerceAPI.Application.Services;
using Moq;
using ECommerceAPI.Application.DTOs.Request;
using FluentAssertions;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Application.Helpers;

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

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowException_WhenEmailExists()
        {
            //Arrange
            var user = new RegisterUserRequestDto
            {
                Email = "test@mail.com",
                Password = "123456789@",
                ConfirmPassword = "123456789@",
                FirstName = "test",
                LastName = "test",
                PhoneNumber = "0123456789"
            };

            _userRepoMock
                .Setup(u => u.EmailExistsAsync(user.Email))
                .ReturnsAsync(true);

            //Act
            Func<Task> act = async () => await _sut.RegisterUserAsync(user);

            //Assert
            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage("Unable to register user");
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldRegister_WhenEveryThingIsValid()
        {
            //Arrange
            var user = new RegisterUserRequestDto
            {
                Email = "test@mail.com",
                Password = "123456789@",
                ConfirmPassword = "123456789@",
                FirstName = "test",
                LastName = "test",
                PhoneNumber = "0123456789"
            };

            _userRepoMock
                .Setup(u => u.EmailExistsAsync(user.Email))
                .ReturnsAsync(false);

            _userRepoMock
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            //Act
            var result = await _sut.RegisterUserAsync(user);

            //Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("test@mail.com");

            _userRepoMock.Verify(
                x => x.AddAsync(It.IsAny<User>()),
                Times.Once
            );
        }

        [Fact]
        public async Task LoginUserAsync_ShouldLogin_WhenEveryThingIsValid()
        {
            //Arrange
            var user = new LoginUserRequestDto
            {
                Email = "test@mail.com",
                Password = "123456789"
            };

            var existingUser = new User
            {
                Email = "test@mail.com",
                PasswordHash = PasswordHasher.Hash("123456789"),
                FirstName = "test",
                LastName = "test",
                Phone = "0123456789",
                CreatedAt = DateTime.UtcNow
            };

            _userRepoMock
                .Setup(u => u.GetByEmailAsync(user.Email))
                .ReturnsAsync(existingUser);

            //Act
            var result = await _sut.LoginUserAsync(user);

            //Assert
            result.Should().NotBeNull();
            result.Email.Should().Be("test@mail.com");
        }


    }
}
