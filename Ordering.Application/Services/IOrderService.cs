using Ordering.Application.DTOs;
using Ordering.Domain.Entities;

namespace Ordering.Application.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync();
        Task<Order> GetOrderAsync(Guid id);
        Task<Order> GetOrderAsync(string userName);
        Task<Order> CreateOrderAsync(BasketDto basketDto, string userName);
        Task DeleteOrderAsync(Guid id);
    }
}