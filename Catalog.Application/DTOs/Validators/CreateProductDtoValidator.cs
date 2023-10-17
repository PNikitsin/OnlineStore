using FluentValidation;

namespace Catalog.Application.DTOs.Validators
{
    public class CreateProductDtoValidator : AbstractValidator<InputProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(createCategoryDto
                => createCategoryDto.Code).NotEmpty().MaximumLength(16);
            RuleFor(createCategoryDto
                => createCategoryDto.Name).NotEmpty().MaximumLength(64);
            RuleFor(createCategoryDto
                => createCategoryDto.Name).NotEmpty().MaximumLength(512);
            RuleFor(createCategoryDto
                => createCategoryDto.Price).NotEmpty().NotEqual(0);
            RuleFor(createCategoryDto
                => createCategoryDto.CategoryId).NotEmpty().NotEqual(0);
        }
    }
}