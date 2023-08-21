using AutoMapper;
using Ordering.Application.DTOs;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.Interfaces;

namespace Ordering.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetAllAsync();

            return orders;
        }

        public async Task<Order> GetOrderAsync(Guid id)
        {
            var order = await _unitOfWork.Orders.GetAsync(order => order.Id == id);

            return order;
        }

        public async Task<Order> GetOrderAsync(string userName)
        {
            var order = await _unitOfWork.Orders.GetAsync(order => order.UserName == userName);

            return order;
        }

        public async Task<Order> CreateOrderAsync(BasketDto basketDto, string userName)
        {
            var order = _mapper.Map<Order>(basketDto);

            order.UserName = userName;
            order.CreatedAt = DateTime.Now;
            order.OrderStatus = OrderStatus.Accepted.ToString();

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitAsync();

            return order;
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await _unitOfWork.Orders.GetAsync(order => order.Id == id)
                ?? throw new NotFoundException("Order not found");

            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.CommitAsync();
        }
    }
}