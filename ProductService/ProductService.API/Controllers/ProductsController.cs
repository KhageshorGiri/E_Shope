using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Dtos;
using ProductService.Application.ServiceInterfaces;

namespace ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        // GET : api/Products
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("GET request received for all products.");
            var allProducts = await _productService.GetAllAsync(cancellationToken);
            _logger.LogInformation("Retrieved all products successfully.");
            return Ok(allProducts);
        }

        // GET : api/Products/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GET request received for product with ID {ProductId}.", id);
            var product = await _productService.GetByIdAsync(id, cancellationToken);
            if(product is null)
            {
                return NoContent();
            }
            _logger.LogInformation("Retrieved product with ID {ProductId} successfully.", id);
            return Ok(product);
        }

        // POST : api/Products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductDto product, CancellationToken cancellationToken)
        {
            _logger.LogInformation("POST request received to create a new product {0}.", product);
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid model state received in the POST request {0}.", ModelState.ToString());
                return BadRequest(ModelState);
            }
            await _productService.AddAsync(product, cancellationToken);
            _logger.LogInformation("New product created successfully {0}.", product);
            return Ok("Success.");
        }

        // PUT : api/Products/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProductDto product, CancellationToken cancellationToken)
        {
            _logger.LogInformation("PUT request received to update product with ID {ProductId}.", id);
            if (!ModelState.IsValid)
            {
                _logger.LogInformation("Invalid model state received in the Pust request {0}.", ModelState.ToString());
                return BadRequest(ModelState);
            }
            var existingProduct = await _productService.GetByIdAsync(id, cancellationToken);
            if(existingProduct is null)
            {
                return NoContent();
            }
            await _productService.UpdateAsync(id, product, cancellationToken);
            _logger.LogInformation("Product with ID {ProductId} updated successfully.", id);
            return Ok("Updated");
        }

        // DELETE : api/Products/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DELETE request received to delete product with ID {ProductId}.", id);
            var existingProduct = await _productService.GetByIdAsync(id, cancellationToken);
            if (existingProduct is null)
            {
                return NoContent();
            }
            await _productService.DeleteAsync(id, cancellationToken);
            _logger.LogInformation("Product with ID {ProductId} deleted successfully.", id);
            return Ok("Deleted.");
        }
    }
}
