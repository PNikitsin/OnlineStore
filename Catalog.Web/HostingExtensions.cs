using Catalog.Application.AutoMapper;
using Catalog.Application.Services;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Data.Repositories;
using Catalog.Web.Extensions;
using Catalog.Web.Middleware;

namespace Catalog.Web
{
    public static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddDatabase(builder.Configuration);
            builder.Services.AddAccessToken(builder.Configuration);
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddTransient<IProductService, ProductService>();
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddAutoMapper(typeof(AppMapperProfile));
            builder.Services.AddRedis(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddValidation();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerUI();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }
    }
}