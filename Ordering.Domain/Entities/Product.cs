namespace Ordering.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Guid OrderId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; } = string.Empty;
    }
}