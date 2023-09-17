using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Data.Repositories;

namespace Catalog.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Products = new ProductRepository(_dbContext);
            Categories = new CategoryRepository(_dbContext);
        }

        public IProductRepository Products { get; }
        public ICategoryRepository Categories { get; }

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