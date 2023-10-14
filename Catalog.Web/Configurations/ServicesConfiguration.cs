using Catalog.Application.AutoMapper;
using Catalog.Application.Services.Implementations;
using Catalog.Application.Services.Interfaces;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Data.Mongo;
using Catalog.Infrastructure.Data.Repositories;
using Catalog.Web.Extensions;

namespace Catalog.Web.Configurations
{
    public static class ServicesConfiguration
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;

            builder.Services.AddDatabase(configuration);
            builder.Services.AddAccessToken(configuration);
            builder.Services.AddRedis(configuration);
            builder.Services.AddHangfire(configuration);

            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.Configure<CollectionConfiguration>(builder.Configuration.GetSection("CollectionConfiguration"));
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<IReportService, ReportService>();
            builder.Services.AddAutoMapper(typeof(AppMapperProfile));

            builder.Services.AddRouting(options
                => options.LowercaseUrls = true);
            
            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddValidation();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerUI();

            return builder.Build();
        }
    }
}