using AutoFixture;
using AutoMapper;
using FluentAssertions;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Moq;
using Ordering.Application.DTOs;
using Ordering.Application.Exceptions;
using Ordering.Application.Services.Implementations;
using Ordering.Domain.Entities;
using Ordering.Domain.Interfaces;

namespace OnlineStore.Tests.Ordering.UnitTests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ICacheRepository> _cacheRepositoryMock = new();
        private readonly Mock<IBackgroundJobClient> _backgroundJobClientMock = new();
        private readonly Fixture _fixture = new();
        private readonly CancellationToken _cancellationToken = new();
        private readonly OrderService _orderService;

        public OrderServiceTests()
        {
            _orderService = new OrderService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _httpContextAccessorMock.Object,
                _cacheRepositoryMock.Object,
                _backgroundJobClientMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetOrdersWithСache_ShouldReturnOrders()
        {
            // Arrange
            var ordersCache = _fixture.CreateMany<Order>();

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Order>>("order"))
                    .ReturnsAsync(ordersCache);

            // Act
            var result = await _orderService.GetOrdersAsync();

            // Assert
            result.Should().BeEquivalentTo(ordersCache);
        }

        [Fact]
        public async Task GetOrdersWithoutСache_ShouldReturnOrders()
        {
            // Arrange
            var ordersData = _fixture.CreateMany<Order>();
            IEnumerable<Order>? ordersCache = null;

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Order>?>("order"))
                    .ReturnsAsync(ordersCache);

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Orders.GetAllAsync(_cancellationToken))
                    .ReturnsAsync(ordersData);

            // Act
            var result = await _orderService.GetOrdersAsync();

            // Assert
            result.Should().BeEquivalentTo(ordersData);
        }

        [Fact]
        public async Task GetOrderById_ShouldReturnOrder()
        {
            // Arrange
            var firstOrder = _fixture.Create<Order>();
            var secondOrder = _fixture.Create<Order>();

            var ordersCach = new List<Order>
            {
                firstOrder,
                secondOrder
            };

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Order>>("order"))
                    .ReturnsAsync(ordersCach);

            // Act
            var result = await _orderService.GetOrderAsync(firstOrder.Id);

            // Assert
            result.Should().BeEquivalentTo(firstOrder);
        }

        [Fact]
        public async Task CreateOrder_ShouldReturnCreatedOrder()
        {
            var createOrderDto = _fixture.Create<CreateOrderDto>();
            var userName = "testName";
            var user = _fixture.Create<User>();

            var createdOrderDto = _fixture.Create<Order>();

            _httpContextAccessorMock.Setup(_httpContextAccessorMock =>
                _httpContextAccessorMock.HttpContext.User.Identity.Name)
                    .Returns(userName);

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Users.GetAsync(user => user.UserName == userName, _cancellationToken))
                    .ReturnsAsync(user);

            _mapperMock.Setup(_mapperMock =>
                _mapperMock.Map<CreateOrderDto, Order>(createOrderDto))
                    .Returns(createdOrderDto);

            // Act
            var result = await _orderService.CreateOrderAsync(createOrderDto);

            // Assert
            result.Should().BeEquivalentTo(createdOrderDto);
        }

        [Fact]
        public async Task UpdateOrder_ShouldUpdateOrder()
        {
            // Arrange
            var order = _fixture.Create<Order>();

            var updatedOrderDto = _fixture.Create<UpdateOrderDto>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Orders.GetAsync(order => order.Id == updatedOrderDto.Id, _cancellationToken))
                    .ReturnsAsync(order);

            // Act
            var result = async () => await _orderService.UpdateOrderAsync(updatedOrderDto);

            // Assert
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateOrder_WhenOrderNotFound_ShouldReturnNotFoundException()
        {
            // Arrange 
            Order? order = null;

            var updateOrderDto = _fixture.Create<UpdateOrderDto>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Orders.GetAsync(order => order.Id == updateOrderDto.Id, _cancellationToken))
                    .ReturnsAsync(order);

            // Act
            var result = async () => await _orderService.UpdateOrderAsync(updateOrderDto);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteOrder_ShouldDeleteOrder()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            var order = _fixture.Create<Order>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Orders.GetAsync(order => order.Id == id, _cancellationToken))
                    .ReturnsAsync(order);

            // Act
            var result = async () => await _orderService.DeleteOrderAsync(id);

            // Assert
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task DeleteOrder_WhenOrderNotFound_ShouldReturnNotFoundException()
        {
            // Arrange
            var id = _fixture.Create<Guid>();
            Order? order = null;

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Orders.GetAsync(order => order.Id == id, _cancellationToken))
                    .ReturnsAsync(order);

            // Act
            var result = async () => await _orderService.DeleteOrderAsync(id);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }
    }
}