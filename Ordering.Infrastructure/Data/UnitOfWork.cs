using Ordering.Domain.Interfaces;
using Ordering.Infrastructure.Data.Repositories;

namespace Ordering.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly AppDbContext _dbContext;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Orders = new OrderRepository(_dbContext);
            Users = new UserRepository(_dbContext);
        }

        public IOrderRepository Orders { get; }
        public IUserRepository Users { get; }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task RollbackAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}