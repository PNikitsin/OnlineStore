namespace Ordering.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; } = string.Empty;
        public List<Order> Orders { get; set; } = new();
    }
}