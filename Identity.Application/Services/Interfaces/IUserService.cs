using Identity.Application.DTOs;

namespace Identity.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<RegisterUserDto> UserRegistrationAsync(RegisterUserDto registerUserDto);
        Task<AuthorizationDto> UserAuthorizationAsync(LoginUserDto loginUserDto);
        Task UserDeleteAsync(DeleteUserDto deleteUserDto);
    }
}