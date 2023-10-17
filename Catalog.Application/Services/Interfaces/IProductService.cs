using Catalog.Application.DTOs;

namespace Catalog.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<OutputProductDto>> GetProductsAsync();
        Task<OutputProductDto> GetProductAsync(int id);
        Task<OutputProductDto> CreateProductAsync(InputProductDto product);
        Task<OutputProductDto> UpdateProductAsync(int id, InputProductDto product);
        Task DeleteProductAsync(int id);
    }
}