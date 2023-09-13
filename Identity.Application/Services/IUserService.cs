using Identity.Application.DTOs;

namespace Identity.Application.Services
{
    public interface IUserService
    {
        Task<RegisterUserDto> UserRegistrationAsync(RegisterUserDto registerUserDto);
        Task<AuthorizationDto> UserAuthorizationAsync(LoginUserDto loginUserDto);
        Task UserDeleteAsync(DeleteUserDto deleteUserDto);
    }
}