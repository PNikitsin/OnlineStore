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
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock = new();
        private readonly Fixture _fixture = new();
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _controller = new(_productServiceMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetProducts_ShouldReturnOkObjectResultWithProducts()
        {
            // Arrange
            var products = _fixture.CreateMany<Product>();

            _productServiceMock.Setup(_productServiceMock =>
                _productServiceMock.GetProductsAsync())
                    .ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(products);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOkObjectResultWithProduct()
        {
            // Arrange 
            var id = _fixture.Create<int>();
            var product = _fixture.Create<Product>();

            _productServiceMock.Setup(_productServiceMock =>
                _productServiceMock.GetProductAsync(id))
                    .ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(product);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnOkObjectResultWithCreatedProduct()
        {
            // Arrange
            var product = _fixture.Create<Product>();
            var createProductDto = _fixture.Create<InputProductDto>();

            _productServiceMock.Setup(_categoryServiceMock =>
                _categoryServiceMock.CreateProductAsync(createProductDto))
                    .ReturnsAsync(product);

            // Act
            var result = await _controller.CreateProduct(createProductDto);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            result.As<CreatedAtActionResult>().Value.Should().Be(product);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnOkObjectResultWithUpdatedProduct()
        {
            // Arrange
            var updateProductDto = _fixture.Create<OutputProductDto>();

            // Act
            var result = await _controller.UpdateProduct(updateProductDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            result.As<OkObjectResult>().Value.Should().Be(updateProductDto);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContentResult()
        {
            // Arrange 
            var id = _fixture.Create<int>();

            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }
    }
}