using Catalog.Application.DTOs.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Catalog.Web.Extensions
{
    public static class ValidationExtension
    {
        public static void AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<CreateCategoryDtoValidator>()
                .AddValidatorsFromAssemblyContaining<UpdateCategoryDtoValidator>()
                .AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>()
                .AddValidatorsFromAssemblyContaining<UpdateProductDtoValidator>();
        }
    }
}