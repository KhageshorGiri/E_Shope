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
            _logger.LogInformation("GET request received for all orders.");
            var allOrders = await _orderService.GetAllAsync(cancellationToken);
            _logger.LogInformation("Retrieved all orders successfully.");
            return Ok(allOrders);
        }

        // GET : api/Orders/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GET request received for order with ID {OrderId}.", id);
            var existingOrder = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if(existingOrder is null)
            {
                return NoContent();
            }
            _logger.LogInformation("Retrieved order with ID {OrderId} successfully.", id);
            return Ok(existingOrder);
        }

        // POST : api/Orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateOrderDto order, CancellationToken cancellationToken)
        {
            _logger.LogInformation("POST request received to create a new order {0}.", order);
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid model state received in the POST request {0}.", ModelState.ToString());
                return BadRequest(ModelState);
            }
            await _orderService.AddAsync(order, cancellationToken);
            _logger.LogInformation("New Order created successfully {0}.", order);
            return Ok("Added");
        }

        // PUT : api/Orders/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateOrderDto order, CancellationToken cancellationToken)
        {
            _logger.LogInformation("PUT request received to update order with ID {OrderId}.", id);
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid model state received in the Pust request {0}.", ModelState.ToString());
                return BadRequest(ModelState);
            }
            var existingOrder = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if(existingOrder is null)
            {
                return NoContent();
            }
            await _orderService.UpdateAsync(id, order, cancellationToken);
            _logger.LogInformation("Order with ID {OrderId} updated successfully.", id);
            return Ok("Updated");
        }

        // DELETE : api/Orders/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DELETE request received to delete order with ID {OrderId}.", id);
            var existingOrder = await _orderService.GetOrderByIdAsync(id, cancellationToken);
            if (existingOrder is null)
            {
                return NoContent();
            }
            await _orderService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Order with ID {OrderId} deleted successfully.", id);
            return Ok("Deleted");
        }
    }
}
