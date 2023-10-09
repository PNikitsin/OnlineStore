using AutoFixture;
using AutoMapper;
using Identity.Application.Exceptions;
using FluentAssertions;
using Identity.Application.DTOs;
using Identity.Application.Grpc;
using Identity.Application.Grpc.Protos;
using Identity.Application.Services.Implementations;
using Identity.Application.Services.Interfaces;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace OnlineStore.Tests.Identity.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                new Mock<IUserStore<ApplicationUser>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);

        private readonly Mock<GrpcUser.GrpcUserClient> _clientMock = new();
        private readonly Mock<GrpcUserClient> _grpcClientMock = new();
        private readonly Mock<ITokenService> _tokenServiceMock = new();
        private readonly Mock<IConfiguration> _configurationMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<IPublishEndpoint> _publishEndpointMock = new();
        private readonly Fixture _fixture = new();
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _grpcClientMock = new(_clientMock.Object);

            _userService = new UserService(
                _userManagerMock.Object,
                _grpcClientMock.Object,
                _configurationMock.Object,
                _tokenServiceMock.Object,
                _mapperMock.Object,
                _publishEndpointMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task UserAuthorizationAsync_WhenUserNotFound_ShouldReturnNotFoundException()
        {
            // Arrange
            var loginUserDto = _fixture.Create<LoginUserDto>();
            ApplicationUser? user = null;

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.FindByNameAsync(loginUserDto.UserName))
                    .ReturnsAsync(user);

            // Act
            var result = async () => await _userService.UserAuthorizationAsync(loginUserDto);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UserAuthorizationAsync_WhenUserPasswordIncorrect_ShouldReturnUnauthorizedException()
        {
            // Arrange
            var loginUserDto = _fixture.Create<LoginUserDto>();
            var user = _fixture.Create<ApplicationUser>();
            var checkPasswordResult = false;

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.FindByNameAsync(loginUserDto.UserName))
                    .ReturnsAsync(user);

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.CheckPasswordAsync(user, loginUserDto.Password))
                    .ReturnsAsync(checkPasswordResult);

            // Act
            var result = async () => await _userService.UserAuthorizationAsync(loginUserDto);

            // Assert
            await result.Should().ThrowAsync<UnauthorizedException>();
        }

        [Fact]
        public async Task UserAuthorizationAsync_ShouldReturnToken()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            var loginUserDto = _fixture.Create<LoginUserDto>();
            var checkPasswordResult = true;
            var token = _fixture.Create<string>();
            var secretkey = _fixture.Create<string>();
            var roles = _fixture.Create<IList<string>>();

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.FindByNameAsync(loginUserDto.UserName))
                    .ReturnsAsync(user);

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.CheckPasswordAsync(user, loginUserDto.Password))
                    .ReturnsAsync(checkPasswordResult);

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.GetRolesAsync(user))
                    .ReturnsAsync(roles);

            _configurationMock.Setup(_configurationMock =>
                _configurationMock.GetSection(It.IsAny<string>()).Value)
                    .Returns(secretkey);

            // Act
            var result = await _userService.UserAuthorizationAsync(loginUserDto);

            // Assert
            result.Should().BeOfType<AuthorizationDto>();
        }

        [Fact]
        public async Task UserDelete_WhenUserNotFound_ShouldReturnNotFoundException()
        {
            // Arrange
            ApplicationUser? user = null;
            var deleteUserDto = _fixture.Create<DeleteUserDto>();

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.FindByEmailAsync(deleteUserDto.Email))
                    .ReturnsAsync(user);

            // Act
            var result = async () => await _userService.UserDeleteAsync(deleteUserDto);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UserDelete_WhenPasswordUserIncorrect_ShouldReturnNotFoundException()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            var deleteUserDto = _fixture.Create<DeleteUserDto>();
            var checkPasswordResult = false;

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.FindByEmailAsync(deleteUserDto.Email))
                    .ReturnsAsync(user);

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.CheckPasswordAsync(user, deleteUserDto.Password))
                    .ReturnsAsync(checkPasswordResult);

            // Act
            var result = async () => await _userService.UserDeleteAsync(deleteUserDto);

            // Assert
            await result.Should().ThrowAsync<UnauthorizedException>();
        }

        [Fact]
        public async Task UserDelete_ShouldDeleteUser()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            var deleteUserDto = _fixture.Create<DeleteUserDto>();
            var checkPasswordResult = true;

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.FindByEmailAsync(deleteUserDto.Email))
                    .ReturnsAsync(user);

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.CheckPasswordAsync(user, deleteUserDto.Password))
                    .ReturnsAsync(checkPasswordResult);

            // Act
            var result = async () => await _userService.UserDeleteAsync(deleteUserDto);

            // Assert
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UserRegistrationAsync_WhenUserAlreadyExist_ShouldThrowExistsException()
        {
            // Arrange
            var user = _fixture.Create<ApplicationUser>();
            var registerUserDto = _fixture.Create<RegisterUserDto>();

            _userManagerMock.Setup(_userManagerMock =>
                _userManagerMock.FindByNameAsync(registerUserDto.UserName))
                    .ReturnsAsync(user);

            // Act
            var result = async () => await _userService.UserRegistrationAsync(registerUserDto);

            // Assert
            await result.Should().ThrowAsync<AlreadyExistsException>();
        }
    }
}