using ProductService.Application.Dtos;
using ProductService.Domain.Entities;

namespace ProductService.Application.ServiceInterfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>?> GetAllAsync();
        Task AddAsync(CreateProductDto product);
        Task UpdateAsync(int id, UpdateProductDto product);
        Task DeleteAsync(int id);
    }
}
