using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.DTOs;
using Ordering.Application.Services;

namespace Ordering.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
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
        public async Task<IActionResult> GetOrderAsync(int id)
        {
            var orders = await _orderService.GetOrderAsync(id);

            return Ok(orders);
        }

        [HttpGet("details")]
        public async Task<IActionResult> CheckOrderAsync()
        {
            string userName = User.Identity.Name;
            var order = await _orderService.GetOrderAsync(userName);

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] BasketDto basketDto)
        {
            string userName = User.Identity.Name;
            var order = await _orderService.CreateOrderAsync(basketDto, userName);

            return Ok(order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            await _orderService.DeleteOrderAsync(id);

            return NoContent();
        }
    }
}