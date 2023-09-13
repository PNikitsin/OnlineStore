using Identity.Application.DTOs;
using Identity.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterUserDto registerUserDto)
        {
            var result = await _userService.UserRegistrationAsync(registerUserDto);

            return Ok(result);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(LoginUserDto loginUserDto)
        {
            var authorizationDto = await _userService.UserAuthorizationAsync(loginUserDto);

            return Ok(authorizationDto);
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteAsync(DeleteUserDto deleteUserDto)
        {
            await _userService.UserDeleteAsync(deleteUserDto);

            return NoContent();
        }
    }
}