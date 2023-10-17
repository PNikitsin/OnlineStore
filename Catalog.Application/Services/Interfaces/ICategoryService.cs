using Catalog.Application.DTOs;

namespace Catalog.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<OutputCategoryDto>> GetCategoriesAsync();
        Task<OutputCategoryDto> GetCategoryAsync(int id);
        Task<OutputCategoryDto> CreateCategoryAsync(InputCategoryDto createCategoryDto);
        Task<OutputCategoryDto> UpdateCategoryAsync(int id, InputCategoryDto inputCategoryDto);
        Task DeleteCategoryAsync(int id);
    }
}