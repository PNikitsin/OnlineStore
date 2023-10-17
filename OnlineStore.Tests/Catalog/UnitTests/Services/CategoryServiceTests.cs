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
    public class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ICacheRepository> _cacheRepositoryMock = new();
        private readonly Mock<IBackgroundJobClient> _backgroundJobClientMock = new();
        private readonly Fixture _fixture = new();
        private readonly CancellationToken _cancellationToken = new();
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryService = new CategoryService(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _cacheRepositoryMock.Object,
                _backgroundJobClientMock.Object);

            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task GetCategoriesWithСache_ShouldReturnCategories()
        {
            // Arrange
            var categoriesCache = _fixture.CreateMany<Category>();

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Category>>("category"))
                    .ReturnsAsync(categoriesCache);

            // Act
            var result = await _categoryService.GetCategoriesAsync();

            // Assert
            result.Should().BeEquivalentTo(categoriesCache);
        }

        [Fact]
        public async Task GetCategoriesWithoutСache_ShouldReturnCategories()
        {
            // Arrange
            var categoriesData = _fixture.CreateMany<Category>();
            IEnumerable<Category>? categoryCache = null;

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Category>?>("category"))
                    .ReturnsAsync(categoryCache);

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Categories.GetAllAsync(_cancellationToken))
                    .ReturnsAsync(categoriesData);

            // Act
            var result = await _categoryService.GetCategoriesAsync();

            // Assert
            result.Should().BeEquivalentTo(categoriesData);
        }

        [Fact]
        public async Task GetCategoryById_ShouldReturnCategory()
        {
            // Arrange
            var firstCategory = _fixture.Create<Category>();
            var secondCategory = _fixture.Create<Category>();

            var categoriesCach = new List<Category>
            {
                firstCategory,
                secondCategory
            };

            _cacheRepositoryMock.Setup(_cacheRepositoryMock =>
                _cacheRepositoryMock.GetDataAsync<IEnumerable<Category>>("category"))
                    .ReturnsAsync(categoriesCach);

            // Act
            var result = await _categoryService.GetCategoryAsync(firstCategory.Id);

            // Assert
            result.Should().BeEquivalentTo(firstCategory);
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnCreatedCategory()
        {
            // Arrange
            Category? category = null;

            var createCategoryDto = _fixture.Create<CreateCategoryDto>();
            var createdCategory = _fixture.Create<Category>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Categories.GetAsync(categoryr => categoryr.Name == createCategoryDto.Name, _cancellationToken))
                    .ReturnsAsync(category);

            _mapperMock.Setup(_mapperMock =>
                _mapperMock.Map<CreateCategoryDto, Category>(createCategoryDto))
                    .Returns(createdCategory);

            // Act
            var result = await _categoryService.CreateCategoryAsync(createCategoryDto);

            // Assert
            result.Should().BeEquivalentTo(createdCategory);
        }

        [Fact]
        public async Task CreateCategory_WhenCategoryAlreadyExist_ShouldReturnAlreadyExistException()
        {
            // Arrange 
            var category = _fixture.Create<Category>();
            var createCategoryDto = _fixture.Create<CreateCategoryDto>();

            _mapperMock.Setup(_mapperMock =>
                _mapperMock.Map<CreateCategoryDto, Category>(createCategoryDto))
                    .Returns(category);

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Categories.GetAsync(categoryr => categoryr.Name == createCategoryDto.Name, _cancellationToken))
                    .ReturnsAsync(category);

            // Act
            var result = async () => await _categoryService.CreateCategoryAsync(createCategoryDto);

            // Assert
            await result.Should().ThrowAsync<AlreadyExistsException>();
        }

        [Fact]
        public async Task UpdateCategory_ShouldUpdateCategory()
        {
            // Arrange
            var category = _fixture.Create<Category>();

            var updatedCategoryDto = _fixture.Create<UpdateCategoryDto>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Categories.GetAsync(categoryr => categoryr.Id == updatedCategoryDto.Id, _cancellationToken))
                    .ReturnsAsync(category);

            // Act
            var result = async () => await _categoryService.UpdateCategoryAsync(updatedCategoryDto);

            // Assert
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task UpdateCategory_WhenCategoryNotFound_ShouldReturnNotFoundException()
        {
            // Arrange 
            Category? category = null;

            var updatedCategoryDto = _fixture.Create<UpdateCategoryDto>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Categories.GetAsync(categoryr => categoryr.Id == updatedCategoryDto.Id, _cancellationToken))
                    .ReturnsAsync(category);

            // Act
            var result = async () => await _categoryService.UpdateCategoryAsync(updatedCategoryDto);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteCategory_ShouldDeleteCategory()
        {
            // Arrange
            var id = _fixture.Create<int>();
            var category = _fixture.Create<Category>();

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Categories.GetAsync(categoryr => categoryr.Id == id, _cancellationToken))
                    .ReturnsAsync(category);

            // Act
            var result = async () => await _categoryService.DeleteCategoryAsync(id);

            // Assert
            await result.Should().NotThrowAsync<Exception>();
        }

        [Fact]
        public async Task DeleteCategory_WhenCategoryNotFound_ShouldReturnNotFoundException()
        {
            // Arrange
            var id = _fixture.Create<int>();
            Category? category = null;

            _unitOfWorkMock.Setup(_unitOfWorkMock =>
                _unitOfWorkMock.Categories.GetAsync(categoryr => categoryr.Id == id, _cancellationToken))
                    .ReturnsAsync(category);

            // Act
            var result = async () => await _categoryService.DeleteCategoryAsync(id);

            // Assert
            await result.Should().ThrowAsync<NotFoundException>();
        }
    }
}