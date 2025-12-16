using ECommerceAPI.Application.Services;
using ECommerceAPI.Domain.Entities;
using ECommerceAPI.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ECommerceAPI.Application.Tests
{
    public class OrderServiceTest
    {
        private readonly Mock<IOrderRepository> _orderRepoMock;
        private readonly Mock<IProductRepository> _productRepoMock;
        private readonly Mock<ICartRepository> _cartRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;

        private readonly OrderService _sut; // System Under Test

        public OrderServiceTest()
        {
            _orderRepoMock = new Mock<IOrderRepository>();
            _cartRepoMock = new Mock<ICartRepository>();
            _productRepoMock = new Mock<IProductRepository>();
            _userRepoMock = new Mock<IUserRepository>();

            _sut = new OrderService(
                _orderRepoMock.Object,
                _cartRepoMock.Object,
                _productRepoMock.Object,
                _userRepoMock.Object
            );
        }

        [Fact]
        public async Task CheckoutAsync_ShouldThrowException_WhenCartIsEmpty()
        {
            //Arrange
            int userId = 1;
            string address = "cairo";

            _cartRepoMock
                .Setup(x => x.GetUserCartAsync(userId))
                .ReturnsAsync(new List<CartItem>());

            //Act
            Func<Task> act = async () => await _sut.CheckoutAsync(userId,address);

            //Assert
            await act.Should()
                     .ThrowAsync<InvalidOperationException>()
                     .WithMessage("cart is empty");
        }

        [Fact]
        public async Task CheckoutAsync_ShouldThrowException_WhenProductNotFound()
        {
            //Arrange
            int userId = 1;

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    ProductId = 10,
                    Quantity = 2
                }
            };

            _cartRepoMock
                .Setup(x => x.GetUserCartAsync(userId))
                .ReturnsAsync(cartItems);

            _productRepoMock
                .Setup(x => x.GetByIdAsync(10))
                .ReturnsAsync((Product)null);


            //Act
            Func<Task> act = async () => await _sut.CheckoutAsync(userId, "Cairo");

            //Assert
            await act.Should()
                     .ThrowAsync<InvalidOperationException>()
                     .WithMessage("Product 10 not found");
        }

        [Fact]
        public async Task CheckoutAsync_ShouldCreateOrder_WhenEveryThingIsValid()
        {
            //Arrange
            int userId = 1;

            var cartItems = new List<CartItem>
            {
                new CartItem
                {
                    ProductId = 1,
                    Quantity = 2
                }
            };

            var Product = new Product
            {
                Id = 1,
                Name = "Laptop",
                Price = 30000
            };

            _cartRepoMock
                .Setup(x => x.GetUserCartAsync(userId))
                .ReturnsAsync(cartItems);

            _productRepoMock
                .Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(Product);

            _productRepoMock
                .Setup(x => x.IsInStockAsync(1,2))
                .ReturnsAsync(true);

            _orderRepoMock
                .Setup(x => x.GenerateOrderNumberAsync())
                .ReturnsAsync("ORD123");

            _orderRepoMock
                .Setup(x => x.AddAsync(It.IsAny<Order>()))
                .ReturnsAsync((Order o) => o);

            //Act
            var result = await _sut.CheckoutAsync(userId, "cairo");

            //Assert
            result.Should().NotBeNull();
            result.Total.Should().Be(60000);
            result.OrderNumber.Should().Be("ORD123");

            _productRepoMock.Verify(x => x.ReduceStockAsync(1, 2), Times.Once);
            _cartRepoMock.Verify(x => x.ClearUserCartAsync(userId), Times.Once);
        }

    }
}
