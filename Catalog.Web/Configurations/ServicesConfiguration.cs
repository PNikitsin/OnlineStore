using Catalog.Application.AutoMapper;
using Catalog.Application.Services;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Data;
using Catalog.Web.Extensions;

namespace Catalog.Web.Configurations
{
    public static class ServicesConfiguration
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddAccessToken(builder.Configuration);
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddAutoMapper(typeof(AppMapperProfile));
            builder.Services.AddControllers();
            builder.Services.AddValidation();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerUI();

            return builder.Build();
        }
    }
}