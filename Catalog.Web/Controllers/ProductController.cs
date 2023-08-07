using Catalog.Application.DTOs;
using Catalog.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsAsync()
        {
            var products = await _productService.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var product = await _productService.GetProductAsync(id);

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductDto productDto)
        {
            var product = await _productService.CreateProductAsync(productDto);

            return Created("get-product", product.Id);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductAsync([FromBody] UpdateProductDto productDto)
        {
            await _productService.UpdateProductAsync(productDto);

            return Ok(productDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            await _productService.DeleteProductAsync(id);

            return NoContent();
        }
    }
}