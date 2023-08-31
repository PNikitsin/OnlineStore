using Ordering.Domain.Enums;

namespace Ordering.Application.DTOs
{
    public class UpdateOrderDto
    {
        public Guid Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}