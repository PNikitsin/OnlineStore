using Catalog.Application.AutoMapper;
using Catalog.Application.Services;
using Catalog.Domain.Interfaces;
using Catalog.Infrastructure.Data;
using Catalog.Web.Extensions;
using Catalog.Web.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

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

var app = builder.Build();

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

app.Run();