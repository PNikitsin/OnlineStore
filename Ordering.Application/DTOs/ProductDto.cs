namespace Ordering.Application.DTOs
{
    public class ProductDto
    {
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}