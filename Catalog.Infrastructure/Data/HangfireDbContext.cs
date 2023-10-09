using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data
{
    public class HangfireDbContext : DbContext
    {
        public HangfireDbContext(DbContextOptions<HangfireDbContext> options)
            : base(options) { }
    }
}