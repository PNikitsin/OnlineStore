using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Ordering.Application.DTOs;
using Ordering.Application.Services.Interfaces;
using Ordering.Domain.Entities;
using Ordering.Web.Controllers;

namespace OnlineStore.Tests.Ordering.UnitTests.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock = new();
        private readonly Fixture _fixture = new();
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _controller = new(_orderServiceMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetOrders_ShouldReturnOkObjectResultWithOrders()
        {
            // Arrange
            var orders = _fixture.CreateMany<Order>();


            _orderServiceMock.Setup(_orderServiceMock =>
                _orderServiceMock.GetOrdersAsync())
                    .ReturnsAsync(orders);

            // Act
            var result = await _controller.GetOrdersAsync();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(orders);
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnOkObjectResultWithOrder()
        {
            // Arrange 
            var id = _fixture.Create<Guid>();
            var order = _fixture.Create<Order>();

            _orderServiceMock.Setup(_orderServiceMock =>
                _orderServiceMock.GetOrderAsync(id))
                    .ReturnsAsync(order);

            // Act
            var result = await _controller.GetOrderAsync(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(order);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnOkObjectResultWithCreatedOrder()
        {
            // Arrange
            var order = _fixture.Create<Order>();
            var createOrderDto = _fixture.Create<CreateOrderDto>();

            _orderServiceMock.Setup(_orderServiceMock =>
                _orderServiceMock.CreateOrderAsync(createOrderDto))
                    .ReturnsAsync(order);

            // Act
            var result = await _controller.CreateOrderAsync(createOrderDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(order);
        }

        [Fact]
        public async Task UpdateOrder_ShouldReturnOkObjectResultWithUpdatedOrder()
        {
            // Arrange
            var order = _fixture.Create<Order>();
            var updateOrderDto = _fixture.Create<UpdateOrderDto>();

            _orderServiceMock.Setup(_orderServiceMock =>
                _orderServiceMock.UpdateOrderAsync(updateOrderDto))
                    .ReturnsAsync(order);

            // Act
            var result = await _controller.UpdateOrderAsync(updateOrderDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(order);
        }

        [Fact]
        public async Task DeleteOrder_ShouldReturnNoContentResult()
        {
            // Arrange 
            var id = _fixture.Create<Guid>();

            // Act
            var result = await _controller.DeleteOrderAsync(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}