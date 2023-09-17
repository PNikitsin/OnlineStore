using Catalog.Infrastructure.Data;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Web.Extensions
{
    public static class HangfireExtension
    {
        public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HangfireConnection");

            services.AddHangfire(hangfire
                => hangfire.UseSqlServerStorage(connectionString));

            services.AddHangfireServer();

            services.AddDbContext<HangfireDbContext>(options
                => options.UseSqlServer(connectionString));

            services.BuildServiceProvider().GetService<HangfireDbContext>()?.Database.Migrate();
        }
    }
}