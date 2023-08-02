using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;

namespace Catalog.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync();

            return categories;
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetAsync(category => category.Id == id);

            return category ?? throw new NotFoundException("Category not found");
        }

        public async Task<Category> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = await _unitOfWork.Categories.GetAsync(name => name.Name == createCategoryDto.Name);

            if (category == null)
            {
                category = _mapper.Map<CreateCategoryDto, Category>(createCategoryDto);

                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.CommitAsync();

                return category;
            }
            else
            {
                throw new AlreadyExistsException("Category already exists");
            }
        }

        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var category = await _unitOfWork.Categories.GetAsync(category => category.Id == updateCategoryDto.Id)
                ?? throw new NotFoundException("Category not found");

            category.Name = updateCategoryDto.Name;
            category.Description = updateCategoryDto.Description;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetAsync(category => category.Id == id)
                ?? throw new NotFoundException("Category not found");
            
            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.CommitAsync();
        }
    }
}