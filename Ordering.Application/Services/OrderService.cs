﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Ordering.Application.DTOs;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using Ordering.Domain.Enums;
using Ordering.Domain.Interfaces;

namespace Ordering.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork; 
        private readonly IMapper _mapper;
        private readonly ICacheRepository _cacheRepository;

        public OrderService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ICacheRepository cacheRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            var orders = await _cacheRepository.GetDataAsync<IEnumerable<Order>>("order");

            if (orders != null)
            {
                return orders;
            }

            orders = await _unitOfWork.Orders.GetAllAsync();
            await _cacheRepository.SetDataAsync("order", orders);

            return orders;
        }

        public async Task<Order> GetOrderAsync(Guid id)
        {
            Order order;
            var orders = await _cacheRepository.GetDataAsync<IEnumerable<Order>>("order");

            if (orders != null)
            {
                order = orders.FirstOrDefault(order => order.Id == order.Id)!;

                if (order != null)
                {
                    return order;
                }
            }

            order = await _unitOfWork.Orders.GetAsync(order => order.Id == id);

            return order ?? throw new NotFoundException("OrderNotFound");
        }

        public async Task<Order> CreateOrderAsync(CreateOrderDto orderDto)
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;

            var user = await _unitOfWork.Users.GetAsync(user => user.UserName == userName);

            var order = _mapper.Map<CreateOrderDto, Order>(orderDto);

            order.CreatedAt = DateTime.Now;
            order.OrderStatus = OrderStatus.Accepted.ToString();
            order.UserId = user.Id;

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CommitAsync();

            await _cacheRepository.RemoveAsync("order");

            return order;
        }

        public async Task<Order> UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            var order = await _unitOfWork.Orders.GetAsync(order => order.Id == updateOrderDto.Id);
            order.OrderStatus = updateOrderDto.Status.ToString();

            _unitOfWork.Orders.Update(order);
            await _unitOfWork.CommitAsync();

            await _cacheRepository.RemoveAsync("order");

            return order;
        }

        public async Task DeleteOrderAsync(Guid id)
        {
            var order = await _unitOfWork.Orders.GetAsync(order => order.Id == id)
                ?? throw new NotFoundException("Order not found");

            _unitOfWork.Orders.Remove(order);
            await _unitOfWork.CommitAsync();

            await _cacheRepository.RemoveAsync("order");
        }
    }
}