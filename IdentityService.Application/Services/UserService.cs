using IdentityService.Application.DTOs;
using IdentityService.Application.Exceptions;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public UserService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<RegisterUserDto> UserRegistrationAsync(RegisterUserDto registerUserDto)
        {
            var user = _userManager.Users.FirstOrDefault(user => user.UserName == registerUserDto.UserName || user.Email == registerUserDto.Email);

            if (user != null)
            {
                throw new AlreadyExistsException("Username or email already taken");
            }

            user = new ApplicationUser
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName
            };

            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            }

            return registerUserDto;
        }

        public async Task<AuthorizationDto> UserAuthorizationAsync(LoginUserDto loginUserDto, string secretKey)
        {
            var user = await _userManager.FindByNameAsync(loginUserDto.UserName);

            if (user == null)
            {
                throw new NotFoundException($"Username is incorrect"); 
            }

            if (!await _userManager.CheckPasswordAsync(user, loginUserDto.Password))
            {
                throw new UnauthorizedException("User password is incorrect");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
               new Claim(ClaimTypes.Name, user.UserName),
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var authorizationDto = new AuthorizationDto()
            {
                UserName = user.UserName,
                Email = user.Email,
                Roles = userRoles.ToList(),
                Token = _tokenService.GenerateToken(authClaims, secretKey)
            };

            return authorizationDto;
        }
    }
}