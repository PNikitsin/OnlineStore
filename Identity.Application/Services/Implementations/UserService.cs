
using Identity.Application.DTOs;
using Identity.Application.Exceptions;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using AutoMapper;
using Identity.Application.Grpc;
using MassTransit;
using OnlineStore.Shared;
using Microsoft.Extensions.Configuration;
using Identity.Application.Services.Interfaces;

namespace Identity.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GrpcUserClient _userClient;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IBus _bus;

        public UserService(
            UserManager<ApplicationUser> userManager,
            GrpcUserClient userClient,
            IConfiguration configuration,
            ITokenService tokenService,
            IMapper mapper,
            IBus bus)
        {
            _userManager = userManager;
            _userClient = userClient;
            _configuration = configuration;
            _tokenService = tokenService;
            _mapper = mapper;
            _bus = bus;
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

            await _userClient.CreateUser(user);

            return registerUserDto;
        }

        public async Task UserDeleteAsync(DeleteUserDto deleteUserDto)
        {
            var user = await _userManager.FindByEmailAsync(deleteUserDto.Email);

            if (user == null)
            {
                throw new NotFoundException($"Email is incorrect");
            }

            if (!await _userManager.CheckPasswordAsync(user, deleteUserDto.Password))
            {
                throw new UnauthorizedException("User password is incorrect");
            }

            await _userManager.DeleteAsync(user);

            var deleteUserMessageDto = _mapper.Map<ApplicationUser, DeleteUserMessageDto>(user);

            var queue = _configuration.GetSection("MessageBroker:userQueue").Value;
            var url = new Uri(queue);

            var enpoint = await _bus.GetSendEndpoint(url);
            await enpoint.Send(deleteUserMessageDto);
        }

        public async Task<AuthorizationDto> UserAuthorizationAsync(LoginUserDto loginUserDto)
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

            var secretKey = _configuration.GetSection("Token:Key").Value!;
            var token = _tokenService.GenerateToken(authClaims, secretKey);

            var authorizationDto = new AuthorizationDto()
            {
                UserName = user.UserName,
                Email = user.Email,
                Roles = userRoles.ToList(),
                Token = token
            };

            return authorizationDto;
        }
    }
}