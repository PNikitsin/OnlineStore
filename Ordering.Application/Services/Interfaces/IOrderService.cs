using Ordering.Application.DTOs;
using Ordering.Domain.Entities;

namespace Ordering.Application.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderAsync(Guid id);
        Task<Order> CreateOrderAsync(CreateOrderDto basketDto);
        Task<Order> UpdateOrderAsync(UpdateOrderDto updateOrderDto);
        Task DeleteOrderAsync(Guid id);
    }
}