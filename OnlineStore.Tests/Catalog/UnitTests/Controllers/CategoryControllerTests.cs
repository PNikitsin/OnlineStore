using AutoFixture;
using Catalog.Application.DTOs;
using Catalog.Application.Services.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace OnlineStore.Tests.Catalog.UnitTests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _categoryServiceMock = new();
        private readonly Fixture _fixture = new();
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _controller = new(_categoryServiceMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetCategories_ShouldReturnOkObjectResultWithCategories()
        {
            // Arrange
            var categories = _fixture.CreateMany<Category>();

            _categoryServiceMock.Setup(_categoryServiceMock =>
                _categoryServiceMock.GetCategoriesAsync())
                    .ReturnsAsync(categories);

            // Act
            var result = await _controller.GetCategories();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(categories);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnOkObjectResultWithCategory()
        {
            // Arrange 
            var id = _fixture.Create<int>();
            var category = _fixture.Create<Category>();

            _categoryServiceMock.Setup(_categoryServiceMock =>
                _categoryServiceMock.GetCategoryAsync(id))
                    .ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategory(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(category);
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnOkObjectResultWithCreatedCategory()
        {
            // Arrange
            var category = _fixture.Create<Category>();
            var createCategoryDto = _fixture.Create<CreateCategoryDto>();

            _categoryServiceMock.Setup(_categoryServiceMock =>
                _categoryServiceMock.CreateCategoryAsync(createCategoryDto))
                    .ReturnsAsync(category);

            // Act
            var result = await _controller.CreateCategory(createCategoryDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(category);
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnOkObjectResultWithUpdatedCategory()
        {
            // Arrange
            var updateCategoryDto = _fixture.Create<UpdateCategoryDto>();

            // Act
            var result = await _controller.UpdateCategory(updateCategoryDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(updateCategoryDto);
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnNoContentResult()
        {
            // Arrange 
            var id = _fixture.Create<int>();

            // Act
            var result = await _controller.DeleteCategory(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}