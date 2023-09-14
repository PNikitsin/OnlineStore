using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.DTOs;
using Ordering.Application.Services.Interfaces;

namespace Ordering.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetOrdersAsync()
        {
            var orders = await _orderService.GetOrdersAsync();

            return Ok(orders);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetOrderAsync(Guid id)
        {
            var orders = await _orderService.GetOrderAsync(id);

            return Ok(orders);
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateOrderAsync([FromBody] UpdateOrderDto updateOrderDto)
        {
            var order = await _orderService.UpdateOrderAsync(updateOrderDto);

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderDto basketDto)
        {
            var order = await _orderService.CreateOrderAsync(basketDto);

            return Ok(order);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteOrderAsync(Guid id)
        {
            await _orderService.DeleteOrderAsync(id);

            return NoContent();
        }
    }
}