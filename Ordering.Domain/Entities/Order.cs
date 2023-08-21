namespace Ordering.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string UserName { get; set; } = null!;
        public decimal? TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string OrderStatus { get; set; } = null!;
    }
}