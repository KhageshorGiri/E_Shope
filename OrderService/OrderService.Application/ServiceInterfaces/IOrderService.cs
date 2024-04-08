using OrderService.Application.Dtos;
using OrderService.Domain.Entities;

namespace OrderService.Application.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(CreateOrderDto order, CancellationToken cancellationToken);
        Task UpdateAsync(int id, UpdateOrderDto order, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
