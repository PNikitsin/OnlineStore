using FluentValidation;

namespace IdentityService.Application.DTOs.Validators
{
    public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
    {
        public LoginUserDtoValidator()
        {
            RuleFor(loginUserDto => loginUserDto.UserName).NotEmpty();
            RuleFor(loginUserDto => loginUserDto.Password).NotEmpty();
        }
    }
}