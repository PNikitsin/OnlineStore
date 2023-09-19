using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Exceptions;
using Catalog.Application.Services.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;
using Hangfire;

namespace Catalog.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICacheRepository _cacheRepository;
        private readonly IBackgroundJobClient _backgroundJobClient;

        public ProductService(
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

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var products = await _cacheRepository.GetDataAsync<IEnumerable<Product>>("product");

            if (products != null)
            {
                return products;
            }

            products = await _unitOfWork.Products.GetAllAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.SetDataAsync("product", products));

            return products;
        }

        public async Task<Product> GetProductAsync(int id)
        {
            Product product = new();
            
            var products = await _cacheRepository.GetDataAsync<IEnumerable<Product>>("product");

            if (products != null)
            {
                product = products.FirstOrDefault(product => product.Id == id)!;
            }

            var productResult = product is null ? await _unitOfWork.Products.GetAsync(product => product.Id == id) : product;

            return productResult ?? throw new NotFoundException("Product not found");
        }

        public async Task<Product> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = await _unitOfWork.Products.GetAsync(name => name.Name == createProductDto.Name);

            if (product != null)
            {
                throw new AlreadyExistsException("Product not found");
            }

            product = _mapper.Map<CreateProductDto, Product>(createProductDto);

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CommitAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.RemoveAsync("product"));

            return product;
        }

        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var product = await _unitOfWork.Products.GetAsync(category => category.Id == updateProductDto.Id)
                ?? throw new NotFoundException("Product not found");

            product.Code = updateProductDto.Code;
            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;

            product.CategoryId = updateProductDto.CategoryId;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.CommitAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.RemoveAsync("product"));
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetAsync(category => category.Id == id)
                ?? throw new NotFoundException("Product not found");

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CommitAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.RemoveAsync("product"));
        }
    }
}