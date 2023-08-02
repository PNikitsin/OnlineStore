using Catalog.Application.DTOs;
using Catalog.Domain.Entities;

namespace Catalog.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(int id);
        Task<Product> CreateProductAsync(CreateProductDto product);
        Task UpdateProductAsync(UpdateProductDto product);
        Task DeleteProductAsync(int id);
    }
}