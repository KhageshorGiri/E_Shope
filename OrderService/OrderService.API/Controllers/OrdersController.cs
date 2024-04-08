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
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        // GET : api/Orders
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var allOrders = await _orderService.GetAllAsync(cancellationToken);
            return Ok(allOrders);
        }

        // GET : api/Orders/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var existingOrder = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if(existingOrder is null)
            {
                return NoContent();
            }
            return Ok(existingOrder);
        }

        // POST : api/Orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderDto order, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _orderService.AddAsync(order, cancellationToken);
            return Ok("Added");
        }

        // PUT : api/Orders/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateOrderDto order, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingOrder = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if(existingOrder is null)
            {
                return NoContent();
            }
            await _orderService.UpdateAsync(id, order, cancellationToken);
            return Ok("Updated");
        }

        // DELETE : api/Orders/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var existingOrder = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if (existingOrder is null)
            {
                return NoContent();
            }
            await _orderService.DeleteAsync(id, cancellationToken);
            return Ok("Deleted");
        }
    }
}
