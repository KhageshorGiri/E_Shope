using OrderService.Application.Dtos;
using OrderService.Domain.Entities;

namespace OrderService.Application.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(int id);
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task AddAsync(CreateOrderDto order);
        Task UpdateAsync(int id, UpdateOrderDto order);
        Task DeleteAsync(int id);
    }
}
