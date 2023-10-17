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

        public async Task<IEnumerable<OutputProductDto>> GetProductsAsync()
        {
            var productsCache = await _cacheRepository.GetDataAsync<IEnumerable<Product>>("product");

            if (productsCache != null)
            {
                return _mapper.Map<IEnumerable<OutputProductDto>>(productsCache);
            }

            var products = await _unitOfWork.Products.GetAllAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.SetDataAsync("product", products));

            return _mapper.Map<IEnumerable<OutputProductDto>>(products);
        }

        public async Task<OutputProductDto> GetProductAsync(int id)
        {
            Product product = new();
            
            var productsCache = await _cacheRepository.GetDataAsync<IEnumerable<Product>>("product");

            if (productsCache != null)
            {
                product = productsCache.FirstOrDefault(product => product.Id == id)!;
            }

            var productResult = product is null ? await _unitOfWork.Products.GetAsync(product => product.Id == id) : product;

            if (productResult == null)
            {
                throw new NotFoundException("Product not found");
            }

            return _mapper.Map<OutputProductDto>(productResult); 
        }

        public async Task<OutputProductDto> CreateProductAsync(InputProductDto inputProductDto)
        {
            var product = await _unitOfWork.Products.GetAsync(product => product.Name == inputProductDto.Name);

            if (product != null)
            {
                throw new AlreadyExistsException("Product already exists");
            }

            product = _mapper.Map<Product>(inputProductDto);

            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CommitAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.RemoveAsync("product"));

            return _mapper.Map<OutputProductDto>(product);
        }

        public async Task<OutputProductDto> UpdateProductAsync(int id, InputProductDto inputProductDto)
        {
            var product = await _unitOfWork.Products.GetAsync(product => product.Id == id)
                ?? throw new NotFoundException("Product not found");

            product.Code = inputProductDto.Code;
            product.Name = inputProductDto.Name;
            product.Description = inputProductDto.Description;
            product.Price = inputProductDto.Price;

            product.CategoryId = inputProductDto.CategoryId;

            _unitOfWork.Products.Update(product);
            await _unitOfWork.CommitAsync();

            _backgroundJobClient.Enqueue(() => _cacheRepository.RemoveAsync("product"));

            return _mapper.Map<OutputProductDto>(product);
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