using FluentValidation;

namespace Identity.Application.DTOs.Validators
{
    public class DeleteUserDtoValidator : AbstractValidator<DeleteUserDto>
    {
        public DeleteUserDtoValidator()
        {
            RuleFor(DeleteUserDto => DeleteUserDto.Email).NotEmpty();
            RuleFor(loginUserDto => loginUserDto.Password).NotEmpty();
        }
    }
}