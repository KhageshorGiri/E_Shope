using ProductService.Domain.Entities;

namespace ProductService.Domain.IRepositories
{
    public interface IProductRepositorycs
    {
        Task<Product?> GetByIdAsync(int id);
        Task<IEnumerable<Product>?> GetAllAsync();
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
    }
}
