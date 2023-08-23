using FluentValidation;
using FluentValidation.AspNetCore;
using Identity.Application.DTOs.Validators;

namespace Identity.Web.Extensions
{
    public static class ValidationExtension
    {
        public static void AddValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<RegisterUserDtoValidator>()
                .AddValidatorsFromAssemblyContaining<LoginUserDtoValidator>()
                .AddValidatorsFromAssemblyContaining<DeleteUserDtoValidator>();
        }
    }
}