using IdentityService.Application.AutoMapper;
using IdentityService.Application.Services;
using IdentityService.Web.Extensions;

namespace IdentityService.Web.Configurations
{
    public static class ServicesConfiguration
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddAutoMapper(typeof(AppMapperProfile));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            return builder.Build();
        }
    }
}