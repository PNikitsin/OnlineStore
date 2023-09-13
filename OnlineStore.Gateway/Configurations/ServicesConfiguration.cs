using Ocelot.DependencyInjection;
using OnlineStore.Gateway.Extensions;

namespace OnlineStore.Gateway.Configurations
{
    public static class ServicesConfiguration
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
            builder.Services.AddOcelot(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCorsPolicy();

            return builder.Build();
        }
    }
}