using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Exceptions;
using Catalog.Application.Services.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Hangfire;

namespace Catalog.Application.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheRepository _cacheRepository;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ICacheRepository cacheRepository)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories = await _cacheRepository.GetDataAsync<IEnumerable<Category>>("category");

            if (categories != null)
            {
                return categories;
            }

            categories = await _unitOfWork.Categories.GetAllAsync();

            BackgroundJob.Enqueue(() => _cacheRepository.SetDataAsync("category", categories));

            return categories;
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            Category category;
            var categories = await _cacheRepository.GetDataAsync<IEnumerable<Category>>("category");

            if (categories != null)
            {
                category = categories.FirstOrDefault(category => category.Id == id)!;

                if (category != null)
                {
                    return category;
                }
            }

            category = await _unitOfWork.Categories.GetAsync(category => category.Id == id);

            return category ?? throw new NotFoundException("Category not found");
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = await _unitOfWork.Categories.GetAsync(name => name.Name == createCategoryDto.Name);

            if (category != null)
            {
                throw new AlreadyExistsException("Category already exists");
            }

            category = _mapper.Map<CreateCategoryDto, Category>(createCategoryDto);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CommitAsync();

            BackgroundJob.Enqueue(() => _cacheRepository.RemoveAsync("category"));

            return category;
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var category = await _unitOfWork.Categories.GetAsync(category => category.Id == updateCategoryDto.Id)
                ?? throw new NotFoundException("Category not found");

            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CommitAsync();

            BackgroundJob.Enqueue(() => _cacheRepository.RemoveAsync("category"));
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetAsync(category => category.Id == id)
                ?? throw new NotFoundException("Category not found");

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.CommitAsync();

            BackgroundJob.Enqueue(() => _cacheRepository.RemoveAsync("category"));
        }
    }
}