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
        public async Task<IActionResult> Get()
        {
            var allProducts = await _productService.GetAllAsync();
            return Ok(allProducts);
        }

        // GET : api/Products/id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = _productService.GetByIdAsync(id);
            if(product is null)
            {
                return NoContent();
            }
            return Ok(product);
        }

        // POST : api/Products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateProductDto product)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _productService.AddAsync(product);
            return Ok("Success.");
        }

        // PUT : api/Products/id
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProductDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = await _productService.GetByIdAsync(id);
            if(existingProduct is null)
            {
                return NoContent();
            }
            await _productService.UpdateAsync(id, product);
            return Ok("Updated");
        }

        // DELETE : api/Products/id
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingProduct = await _productService.GetByIdAsync(id);
            if (existingProduct is null)
            {
                return NoContent();
            }
            await _productService.DeleteAsync(id);
            return Ok("Deleted.");
        }
    }
}
