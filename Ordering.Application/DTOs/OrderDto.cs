using Ordering.Domain.Enums;

namespace Ordering.Application.DTOs
{
    public class OrderDto
    {
        public string UserName { get; set; } = null!;
        public decimal? TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}