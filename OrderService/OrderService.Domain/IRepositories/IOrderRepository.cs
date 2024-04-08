using OrderService.Domain.Entities;

namespace OrderService.Domain.IRepositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(Order order, CancellationToken cancellationToken);
        Task UpdateAsync(Order order, CancellationToken cancellationToken);
        Task DeleteAsync(Order order, CancellationToken cancellationToken);
    }
}
