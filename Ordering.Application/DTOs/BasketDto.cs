namespace Ordering.Application.DTOs
{
    public class BasketDto
    {
        public ICollection<ProductDto> Products { get; set; }
        public decimal TotalPrice
        {
            get
            {
                decimal totalprice = 0;

                foreach (var item in Products)
                {
                    totalprice += item.Price * item.Quantity;
                }

                return totalprice;
            }
        }
    }
}