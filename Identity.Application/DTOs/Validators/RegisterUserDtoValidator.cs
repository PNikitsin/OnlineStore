using FluentValidation;

namespace Identity.Application.DTOs.Validators
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator()
        {
            RuleFor(loginUserDto => loginUserDto.FirstName).NotEmpty();
            RuleFor(loginUserDto => loginUserDto.LastName).NotEmpty();
            RuleFor(loginUserDto => loginUserDto.UserName).NotEmpty();
            RuleFor(loginUserDto => loginUserDto.Email).NotEmpty();
            RuleFor(loginUserDto => loginUserDto.Password).NotEmpty();
        }
    }
}