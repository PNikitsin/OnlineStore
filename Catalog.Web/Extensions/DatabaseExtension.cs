using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Web.Extensions
{
    public static class DatabaseExtension
    {
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options
                => options.UseSqlServer(connectionString));

            services.BuildServiceProvider().GetService<AppDbContext>()?.Database.Migrate();
        }
    }
}