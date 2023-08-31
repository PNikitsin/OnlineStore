using Identity.Application.AutoMapper;
using Identity.Application.Services;
using Identity.Web.Extensions;

namespace Identity.Web.Configurations
{
    public static class ServicesConfiguration
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddGrpcClient(builder.Configuration);
            builder.Services.AddAutoMapper(typeof(AppMapperProfile));
            builder.Services.AddMessageBroker(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            return builder.Build();
        }
    }
}