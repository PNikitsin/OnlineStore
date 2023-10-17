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
        private readonly IBackgroundJobClient _backgroundJobClient;

        public CategoryService(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ICacheRepository cacheRepository, 
            IBackgroundJobClient backgroundJobClient)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cacheRepository = cacheRepository;
            _backgroundJobClient = backgroundJobClient;
        }

        public async Task<IEnumerable<OutputCategoryDto>> GetCategoriesAsync()
        {
            var categoriesCache = await _cacheRepository.GetDataAsync<IEnumerable<Category>>("category");

            if (categoriesCache != null)
            {
                return _mapper.Map<IEnumerable<OutputCategoryDto>>(categoriesCache);
            }

            var categories = await _unitOfWork.Categories.GetAllAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.SetDataAsync("category", categories));

            return _mapper.Map<IEnumerable<OutputCategoryDto>>(categories);
        }

        public async Task<OutputCategoryDto> GetCategoryAsync(int id)
        {
            Category category = new();

            var categoriesCache = await _cacheRepository.GetDataAsync<IEnumerable<Category>>("category");

            if (categoriesCache != null)
            {
                category = categoriesCache.FirstOrDefault(category => category.Id == id)!;
            }

            var categoryResult = category is null ? await _unitOfWork.Categories.GetAsync(category => category.Id == id) : category;

            if (categoryResult == null)
            {
                throw new NotFoundException("Category not found");
            }

            return _mapper.Map<OutputCategoryDto>(categoryResult);
        }

        public async Task<OutputCategoryDto> CreateCategoryAsync(InputCategoryDto inputCategoryDto)
        {
            var category = await _unitOfWork.Categories.GetAsync(category => category.Name == inputCategoryDto.Name);

            if (category != null)
            {
                throw new AlreadyExistsException("Category already exists");
            }

            category = _mapper.Map<Category>(inputCategoryDto);

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.CommitAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.RemoveAsync("category"));

            return _mapper.Map<OutputCategoryDto>(category);
        }

        public async Task<OutputCategoryDto> UpdateCategoryAsync(int id, InputCategoryDto inputCategoryDto)
        {
            var category = await _unitOfWork.Categories.GetAsync(category => category.Id == id)
                ?? throw new NotFoundException("Category not found");

            category.Name = inputCategoryDto.Name;
            category.Description = inputCategoryDto.Description;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CommitAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.RemoveAsync("category"));

            return _mapper.Map<OutputCategoryDto>(category);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetAsync(category => category.Id == id)
                ?? throw new NotFoundException("Category not found");

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.CommitAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.RemoveAsync("category"));
        }
    }
}