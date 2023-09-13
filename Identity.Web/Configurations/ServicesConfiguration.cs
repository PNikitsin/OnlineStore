using Identity.Application.AutoMapper;
using Identity.Application.Services.Implementations;
using Identity.Application.Services.Interfaces;
using Identity.Web.Extensions;

namespace Identity.Web.Configurations
{
    public static class ServicesConfiguration
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddGrpcClient(builder.Configuration);
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddAutoMapper(typeof(AppMapperProfile));
            builder.Services.AddControllers();
            builder.Services.AddValidation();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMessageBroker(builder.Configuration);

            return builder.Build();
        }
    }
}