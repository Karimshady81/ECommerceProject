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
    internal class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        //public async Task<OrderResponseDto> CreateUserOrderAsync(CreateOrderRequestDto orderDto)
        //{                    
        //    //Check if the product exists
        //    var product = await _productRepository.GetByIdAsync(orderDto.ProductId);
        //    if (product == null)
        //    {
        //        throw new InvalidOperationException($"No product with this Id: {orderDto.ProductId}");
        //    }

        //    //Check stock availability
        //    if (!await _productRepository.IsInStockAsync(orderDto.ProductId, orderDto.quantity))
        //        throw new InvalidOperationException("Not enough in stock");

        //    //Generate an order number
        //    var orderNumber = await _orderRepository.GenerateOrderNumberAsync();

        //    //Create order entity
        //    var order = new Order
        //    {
        //        UserId = orderDto.UserId,
        //        OrderNumber = orderNumber,
        //        Status = OrderStatus.Pending,
        //        ShippingAddress = orderDto.ShippingAddress,
        //        CreatedAt = DateTime.UtcNow,
        //        OrderItems = new List<OrderItem>()
        //    };

        //    //Add order_item
        //    var orderItem = new OrderItem
        //    {
        //        ProductId = product.Id,
        //        Quantity = orderDto.quantity,
        //        UnitPrice = product.Price,
        //        TotalPrice = product.Price * orderDto.quantity
        //    };

        //    order.OrderItems.Add(orderItem);

        //    //Calculate total order
        //    order.Total = order.OrderItems.Sum(i => i.TotalPrice);

        //    //Reduce stock
        //    await _productRepository.ReduceStockAsync(product.Id, orderDto.quantity);

        //    //save to database
        //    var createdOrder = await _orderRepository.AddAsync(order);

        //    //Return DTO response
        //    return new OrderResponseDto
        //    {
        //        OrderId = createdOrder.Id,
        //        UserId = createdOrder.UserId,
        //        OrderNumber = createdOrder.OrderNumber,
        //        Total = createdOrder.Total,
        //        Status = createdOrder.Status,
        //        ShippingAddress = createdOrder.ShippingAddress,
        //        CreatedAt = createdOrder.CreatedAt.ToString("B"),
        //    };            
        //}

        public async Task<OrderResponseDto> GetOrderWithDetailsAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            
            if (order == null)
            {
                throw new InvalidOperationException($"No order with this Id: {orderId}");
            }

            return new OrderResponseDto
            {
                OrderId = order.Id,
                UserId = order.UserId,
                OrderNumber = order.OrderNumber,
                Total = order.Total,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                CreatedAt = order.CreatedAt.ToString("B"),
                OrderItems = order.OrderItems.Select(item => new OrderItemsResponseDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice,
                }).ToList()
            };
        }

        public async Task<IEnumerable<OrderResponseDto>> GetOrdersByUser(int userId)
        {
            var user = await _userRepository.GetUserWithOrderAsync(userId);
            
            if (user == null)
                throw new InvalidOperationException($"No user with this Id: {userId}");

            if (!user.Orders.Any())
                throw new InvalidOperationException("No orders by user");

            var response = new List<OrderResponseDto>();

            foreach(var item in user.Orders)
            {
                response.Add(new OrderResponseDto
                {
                    OrderId = item.Id,
                    UserId = item.UserId,
                    OrderNumber = item.OrderNumber,
                    Total = item.Total,
                    Status = item.Status,
                    ShippingAddress = item.ShippingAddress,
                    CreatedAt = item.CreatedAt.ToString("D"),
                });
            }

            return response;
        }

        public async Task<OrderResponseDto> GetOrderByNumber(string orderNumber)
        {
            var order = await _orderRepository.GetOrderByNumberAsync(orderNumber);

            if (order == null)
                throw new InvalidOperationException($"No order with this number: {orderNumber}");

            return new OrderResponseDto
            {
                OrderId = order.Id,
                OrderNumber = order.OrderNumber,
                Total = order.Total,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                CreatedAt = order.CreatedAt.ToString("B"),
                OrderItems = order.OrderItems.Select(item => new OrderItemsResponseDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice,
                }).ToList()
            };
            
        }

        public async Task<IEnumerable<OrderResponseDto>> GetOrderByStatusAsync(OrderStatus status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);

            if (!orders.Any())
                throw new InvalidOperationException($"No order with this status: {status}");

            var response = new List<OrderResponseDto>();

            foreach (var order in orders)
            {
                response.Add(new OrderResponseDto
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    OrderNumber = order.OrderNumber,
                    Total = order.Total,
                    Status = order.Status,
                    ShippingAddress = order.ShippingAddress,
                    CreatedAt = order.CreatedAt.ToString("D"),
                });
            }

            return response;
        }

        public async Task<OrderResponseDto> CheckoutAsync(int userId, string shippingAddress)
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            if (!cartItems.Any())
                throw new InvalidOperationException("Cart is empty");

            //Validate products and stock
            foreach(var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product == null)
                    throw new InvalidOperationException($"Product {item.ProductId} not found");

                if (!await _productRepository.IsInStockAsync(item.ProductId, item.Quantity))
                    throw new InvalidOperationException($"Not enough stock for product {product.Name}");
            }

            //Generate order
            var orderNumber = await _orderRepository.GenerateOrderNumberAsync();

            var order = new Order
            {
                UserId = userId,
                OrderNumber = orderNumber,
                Status = OrderStatus.Pending,
                ShippingAddress = shippingAddress,
                CreatedAt = DateTime.UtcNow,
                OrderItems = new List<OrderItem>()
            };

            //Add order items
            foreach(var item in cartItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * item.Quantity
                });

                //Reduce stock
                await _productRepository.ReduceStockAsync(product.Id, item.Quantity);
            }

            //Calculate total
            order.Total = order.OrderItems.Sum(i => i.TotalPrice);

            //Save order
            var createdOrder = await _orderRepository.AddAsync(order);

            //clear cart
            await _cartRepository.ClearUserCartAsync(userId);

            //Return order DTO
            return new OrderResponseDto
            {
                OrderId = createdOrder.Id,
                UserId = createdOrder.UserId,
                OrderNumber = createdOrder.OrderNumber,
                Total = createdOrder.Total,
                Status = createdOrder.Status,
                ShippingAddress = createdOrder.ShippingAddress,
                CreatedAt = createdOrder.CreatedAt.ToString("B")
            };
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                throw new InvalidOperationException($"No order with this Id: {orderId}");

            return await _orderRepository.DeleteAsync(orderId);
        }
    }
}
