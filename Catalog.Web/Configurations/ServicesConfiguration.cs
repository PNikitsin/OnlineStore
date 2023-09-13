using Catalog.Application.AutoMapper;
using Catalog.Application.Services.Implementations;
using Catalog.Application.Services.Interfaces;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Data.Mongo;
using Catalog.Infrastructure.Data.Repositories;
using Catalog.Web.Extensions;

namespace Catalog.Web.Configurations
{
    public static class ServicesConfiguration
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddAccessToken(builder.Configuration);
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.Configure<CollectionConfiguration>(builder.Configuration.GetSection("CollectionConfiguration"));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<IReportService, ReportService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddAutoMapper(typeof(AppMapperProfile));
            builder.Services.AddRedis(builder.Configuration);
            builder.Services.AddHangfire(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddValidation();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerUI();

            return builder.Build();
        }
    }
}