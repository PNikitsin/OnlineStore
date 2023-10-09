using Ordering.Domain.Entities;
using Ordering.Domain.Interfaces;

namespace Ordering.Infrastructure.Data.Repositories
{
    internal class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext dbContext)
            : base(dbContext) { }
    }
}