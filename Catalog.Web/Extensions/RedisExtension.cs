namespace Catalog.Web.Extensions
{
    public static class RedisExtension 
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration["Redis:Uri"];
            });
        }
    }
}