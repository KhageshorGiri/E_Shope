using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProductService.Application.Dtos;
using ProductService.Application.ServiceInterfaces;
using ProductService.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET : api/Products
        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var allProducts = await _productService.GetAllAsync(cancellationToken);
            return Ok(allProducts);
        }

        // GET : api/Products/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var product = _productService.GetByIdAsync(id, cancellationToken);
            if(product is null)
            {
                return NoContent();
            }
            return Ok(product);
        }

        // POST : api/Products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductDto product, CancellationToken cancellationToken)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _productService.AddAsync(product, cancellationToken);
            return Ok("Success.");
        }

        // PUT : api/Products/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProductDto product, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = await _productService.GetByIdAsync(id, cancellationToken);
            if(existingProduct is null)
            {
                return NoContent();
            }
            await _productService.UpdateAsync(id, product, cancellationToken);
            return Ok("Updated");
        }

        // DELETE : api/Products/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var existingProduct = await _productService.GetByIdAsync(id, cancellationToken);
            if (existingProduct is null)
            {
                return NoContent();
            }
            await _productService.DeleteAsync(id, cancellationToken);
            return Ok("Deleted.");
        }
    }
}
