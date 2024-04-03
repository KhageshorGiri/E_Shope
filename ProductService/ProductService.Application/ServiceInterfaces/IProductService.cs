using ProductService.Application.Dtos;
using ProductService.Domain.Entities;

namespace ProductService.Application.ServiceInterfaces
{
    public interface IProductService
    {
        Task<ProductDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<ProductDto>?> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(CreateProductDto product, CancellationToken cancellationToken);
        Task UpdateAsync(int id, UpdateProductDto product, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
