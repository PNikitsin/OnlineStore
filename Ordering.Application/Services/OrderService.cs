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

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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

            if (order == null)
            {
                throw new NotFoundException("OrderNotFound");
            }

            return order;
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

            return order;
        }

        public async Task<Order> UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            var order = await _unitOfWork.Orders.GetAsync(order => order.Id == updateOrderDto.Id);
            order.OrderStatus = updateOrderDto.Status.ToString();

            _unitOfWork.Orders.Update(order);
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