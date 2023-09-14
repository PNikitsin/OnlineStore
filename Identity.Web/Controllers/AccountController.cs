using Identity.Application.DTOs;
using Identity.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AccountController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
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
            var secretKey = _configuration.GetValue<string>("Token:Key");
            var token = await _userService.UserAuthorizationAsync(loginUserDto, secretKey);

            return Ok(token);
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