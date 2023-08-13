using IdentityService.Application.DTOs;

namespace IdentityService.Application.Services
{
    public interface IUserService
    {
        Task<RegisterUserDto> UserRegistrationAsync(RegisterUserDto registerUserDto);
        Task<AuthorizationDto> UserAuthorizationAsync(LoginUserDto loginUserDto, string secretKey);
    }
}