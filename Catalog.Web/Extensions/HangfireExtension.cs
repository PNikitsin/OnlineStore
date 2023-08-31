using Hangfire;

namespace Catalog.Web.Extensions
{
    public static class HangfireExtension
    {
        public static void AddHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HangfireConnection");

            services.AddHangfire(h => h.UseSqlServerStorage(connectionString));
            services.AddHangfireServer();
        }
    }
}