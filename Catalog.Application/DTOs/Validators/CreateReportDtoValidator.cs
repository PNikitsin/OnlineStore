using FluentValidation;

namespace Catalog.Application.DTOs.Validators
{
    public class CreateReportDtoValidator : AbstractValidator<InputReportDto>
    {
        public CreateReportDtoValidator()
        {
            RuleFor(createCategoryDto
                => createCategoryDto.Theme).NotEmpty().MaximumLength(64);
            RuleFor(createCategoryDto
                => createCategoryDto.Description).NotEmpty().MaximumLength(512);
        }
    }
}