using Ordering.Application.AutoMapper;
using Ordering.Application.Services.Implementations;
using Ordering.Application.Services.Interfaces;
using Ordering.Domain.Interfaces;
using Ordering.Infrastructure.Data;
using Ordering.Web.Extensions;

namespace Ordering.Web.Configurations
{
    public static class ServicesConfiguration
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddAccessToken(builder.Configuration);
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddAutoMapper(typeof(AppMapperProfile));
            builder.Services.AddGrpc();
            builder.Services.AddHangfire(builder.Configuration);
            builder.Services.AddRedis(builder.Configuration);
            builder.Services.AddMessageBroker(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerUI();

            return builder.Build();
        }
    }
}