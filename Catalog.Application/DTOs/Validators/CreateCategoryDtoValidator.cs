using FluentValidation;

namespace Catalog.Application.DTOs.Validators
{
    public class CreateCategoryDtoValidator : AbstractValidator<CreateCategoryDto>
    {
        public CreateCategoryDtoValidator()
        {
            RuleFor(createCategoryDto
                => createCategoryDto.Name).NotEmpty().MaximumLength(64);
            RuleFor(createCategoryDto
                => createCategoryDto.Name).NotEmpty().MaximumLength(512);
        }
    }
}