using AutoFixture;
using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Exceptions;
using Catalog.Application.Services.Implementations;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using FluentAssertions;
using Hangfire;
using Moq;

namespace OnlineStore.Tests.Catalog.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ICacheRepository> _cacheRepositoryMock = new();
        private readonly Mock<IBackgroundJobClient> _backgroundJobClientMock = new();
        private readonly Fixture _fixture = new();
        private readonly CancellationToken _cancellationToken = new();
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productService = new ProductService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _cacheRepositoryMock.Object,
                _backgroundJobClientMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetProductsWithСache_ShouldReturnProducts()
        {
            // Arrange
            var productsCache = _fixture.CreateMany<Product>();

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Product>>("product"))
                    .ReturnsAsync(productsCache);

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            result.Should().BeEquivalentTo(productsCache);
        }

        [Fact]
        public async Task GetProductsWithoutСache_ShouldReturnProducts()
        {
            // Arrange
            var productsData = _fixture.CreateMany<Product>();
            IEnumerable<Product>? productsCache = null;

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Product>?>("product"))
                    .ReturnsAsync(productsCache);

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Products.GetAllAsync(_cancellationToken))
                    .ReturnsAsync(productsData);

            // Act
            var result = await _productService.GetProductsAsync();

            // Assert
            result.Should().BeEquivalentTo(productsData);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct()
        {
            // Arrange
            var firstProduct = _fixture.Create<Product>();
            var secondProduct = _fixture.Create<Product>();

            var productsCach = new List<Product>
            {
                firstProduct,
                secondProduct
            };

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Product>>("product"))
                    .ReturnsAsync(productsCach);

            // Act
            var result = await _productService.GetProductAsync(firstProduct.Id);

            // Assert
            result.Should().BeEquivalentTo(firstProduct);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnCreatedProduct()
        {
            // Arrange
            Product? product = null;

            var createProductDto = _fixture.Create<InputProductDto>();
            var createdProduct = _fixture.Create<Product>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Products.GetAsync(product => product.Name == createProductDto.Name, _cancellationToken))
                    .ReturnsAsync(product);

            _mapperMock.Setup(_mapperMock =>
                _mapperMock.Map<InputProductDto, Product>(createProductDto))
                    .Returns(createdProduct);

            // Act
            var result = await _productService.CreateProductAsync(createProductDto);

            // Assert
            result.Should().BeEquivalentTo(createdProduct);
        }

        [Fact]
        public async Task CreateProduct_WhenProductAlreadyExist_ShouldReturnAlreadyExistException()
        {
            // Arrange 
            var product = _fixture.Create<Product>();
            var createProductDto = _fixture.Create<InputProductDto>();

            _mapperMock.Setup(_mapperMock =>
                _mapperMock.Map<InputProductDto, Product>(createProductDto))
                    .Returns(product);

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Products.GetAsync(product => product.Name == createProductDto.Name, _cancellationToken))
                    .ReturnsAsync(product);

            // Act
            var result = async () => await _productService.CreateProductAsync(createProductDto);

            // Assert
            await result.Should().ThrowAsync<AlreadyExistsException>();
        }

        [Fact]
        public async Task UpdateProduct_ShouldUpdateProduct()
        {
            // Arrange
            var product = _fixture.Create<Product>();

            var updatedProductDto = _fixture.Create<OutputProductDto>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Products.GetAsync(product => product.Id == updatedProductDto.Id, _cancellationToken))
                    .ReturnsAsync(product);

            // Act
            var result = async () => await _productService.UpdateProductAsync(updatedProductDto);

            // Assert
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateProduct_WhenProductNotFound_ShouldReturnNotFoundException()
        {
            // Arrange 
            Product? product = null;

            var updatedProductDto = _fixture.Create<OutputProductDto>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Products.GetAsync(product => product.Id == updatedProductDto.Id, _cancellationToken))
                    .ReturnsAsync(product);

            // Act
            var result = async () => await _productService.UpdateProductAsync(updatedProductDto);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteProduct_ShouldDeleteProduct()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var product = _fixture.Create<Product>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Products.GetAsync(product => product.Id == id, _cancellationToken))
                    .ReturnsAsync(product);

            // Act
            var result = async () => await _productService.DeleteProductAsync(id);

            // Assert
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task DeleteCategory_WhenCategoryNotFound_ShouldReturnNotFoundException()
        {
            // Arrange
            var id = _fixture.Create<int>();
            Product? product = null;

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Products.GetAsync(product => product.Id == id, _cancellationToken))
                    .ReturnsAsync(product);

            // Act
            var result = async () => await _productService.DeleteProductAsync(id);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }
    }
}