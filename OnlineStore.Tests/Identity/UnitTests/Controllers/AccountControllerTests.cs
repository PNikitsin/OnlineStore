using AutoFixture;
using FluentAssertions;
using Identity.Application.DTOs;
using Identity.Application.Services.Interfaces;
using Identity.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace OnlineStore.Tests.Identity.UnitTests.Controllers
{
    public class AccountControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock = new();
        private readonly Fixture _fixture = new();
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _controller = new(_userServiceMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnOkObjectResultWithCreatedUser()
        {
            // Arrange
            var registerUserDto = _fixture.Create<RegisterUserDto>();

            _userServiceMock.Setup(userService =>
                userService.UserRegistrationAsync(registerUserDto))
            .ReturnsAsync(registerUserDto);

            // Act
            var result = await _controller.RegisterAsync(registerUserDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(registerUserDto);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnOkObjectResultWithToken()
        {
            // Arrange
            var authorizationDto = _fixture.Create<AuthorizationDto>();
            var loginUserDto = _fixture.Create<LoginUserDto>();

            _userServiceMock.Setup(userService =>
                userService.UserAuthorizationAsync(loginUserDto))
            .ReturnsAsync(authorizationDto);

            // Act
            var result = await _controller.LoginAsync(loginUserDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(authorizationDto);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNoContentResult()
        {
            // Arrange
            var deleteUserDto = _fixture.Create<DeleteUserDto>();

            // Act
            var result = await _controller.DeleteAsync(deleteUserDto);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}