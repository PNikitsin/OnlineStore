using Identity.Application.DTOs;

namespace Identity.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<RegisterUserDto> UserRegistrationAsync(RegisterUserDto registerUserDto);
        Task UserDeleteAsync(DeleteUserDto deleteUserDto);
        Task<AuthorizationDto> UserAuthorizationAsync(LoginUserDto loginUserDto, string secretKey);
    }
}