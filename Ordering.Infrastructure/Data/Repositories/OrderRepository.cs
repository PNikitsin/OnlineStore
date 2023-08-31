using Ordering.Domain.Entities;
using Ordering.Domain.Interfaces;

namespace Ordering.Infrastructure.Data.Repositories
{
    internal class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext dbContext)
            : base(dbContext) { }
    }
}