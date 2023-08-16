using Ordering.Domain.Enums;

namespace Ordering.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public decimal? TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OrderStatus { get; set; } = null!;
    }
}