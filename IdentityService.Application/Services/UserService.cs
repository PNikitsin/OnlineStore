using Identity.Application.DTOs;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AutoMapper;

namespace Identity.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<RegisterUserDto> UserRegistrationAsync(RegisterUserDto registerUserDto)
        {
            var user = _userManager.Users.FirstOrDefault(user => user.UserName == registerUserDto.UserName || user.Email == registerUserDto.Email);

            if (user != null)
            {
                throw new AlreadyExistsException("Username or email already taken");
            }

            user = _mapper.Map<RegisterUserDto, ApplicationUser>(registerUserDto);

            await _userManager.CreateAsync(user, registerUserDto.Password);

            await _userManager.AddToRoleAsync(user, Roles.User.ToString());

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