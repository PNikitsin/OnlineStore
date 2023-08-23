namespace Ordering.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OrderStatus { get; set; } = string.Empty;

        public Guid UserId { get; set; }
    }
}