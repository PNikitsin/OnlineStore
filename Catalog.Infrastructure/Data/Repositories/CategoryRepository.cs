using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;

namespace Catalog.Infrastructure.Data.Repositories
{
    internal class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext)
           : base(dbContext) { }
    }
}