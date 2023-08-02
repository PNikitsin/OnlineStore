using AutoMapper;
using Catalog.Application.DTOs;
using Catalog.Application.Exceptions;
using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces;

namespace Catalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _unitOfWork.Products.GetAllAsync();
        }

        public async Task<Product> GetProductAsync(int id)
        {
            var category = await _unitOfWork.Products.GetAsync(category => category.Id == id);

            return category ?? throw new NotFoundException("Product not found");
        }

        public async Task<Product> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = await _unitOfWork.Products.GetAsync(name => name.Name == createProductDto.Name);

            if (product == null)
            {
                product = _mapper.Map<CreateProductDto, Product>(createProductDto);

                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.CommitAsync();

                return product;
            }
            else
                throw new AlreadyExistsException("Product not found");
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
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.Products.GetAsync(category => category.Id == id)
                ?? throw new NotFoundException("Product not found");

            _unitOfWork.Products.Remove(product);
            await _unitOfWork.CommitAsync();
        }
    }
}