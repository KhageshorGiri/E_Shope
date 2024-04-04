using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Dtos;
using OrderService.Application.ServiceInterfaces;


namespace OrderService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET : api/Orders
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var allOrders = await _orderService.GetAllAsync();
            return Ok(allOrders);
        }

        // GET : api/Orders/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var existingOrder = await _orderService.GetOrderByIdAsync(id);
            if(existingOrder is null)
            {
                return NoContent();
            }
            return Ok(existingOrder);
        }

        // POST : api/Orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderDto order)
        {
            await _orderService.AddAsync(order);
            return Ok("Added");
        }

        // PUT : api/Orders/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateOrderDto order)
        {
            var existingOrder = _orderService.GetOrderByIdAsync(id);
            if(existingOrder is null)
            {
                return NoContent();
            }
            await _orderService.UpdateAsync(id, order);
            return Ok("Updated");
        }

        // DELETE : api/Orders/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingOrder = _orderService.GetOrderByIdAsync(id);
            if (existingOrder is null)
            {
                return NoContent();
            }
            await _orderService.DeleteAsync(id);
            return Ok("Deleted");
        }
    }
}
