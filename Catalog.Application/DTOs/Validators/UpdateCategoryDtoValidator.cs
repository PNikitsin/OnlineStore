using FluentValidation;

namespace Catalog.Application.DTOs.Validators
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(updateCategoryDto
                => updateCategoryDto.Id).NotEmpty().NotEqual(0);
            RuleFor(updateCategoryDto
                => updateCategoryDto.Name).NotEmpty().MaximumLength(64);
            RuleFor(updateCategoryDto
                => updateCategoryDto.Description).NotEmpty().MaximumLength(512);
        }
    }
}