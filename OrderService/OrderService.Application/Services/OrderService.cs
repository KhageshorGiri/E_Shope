using OrderService.Application.Dtos;
using OrderService.Application.MappingProfile;
using OrderService.Application.ServiceInterfaces;
using OrderService.Domain.IRepositories;

namespace OrderService.Application.Services
{
    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var allOrders = await _orderRepository.GetAllAsync();
            return allOrders.Select(order => order.ToOrderDto());
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order is null)
            {
                throw new ArgumentOutOfRangeException($"Order with Id {id} not found.");
            }
            return order.ToOrderDto();
        }

        public async Task AddAsync(CreateOrderDto order)
        {
            var orderToAdd = order.ToOrder();
            orderToAdd.CreatedBy = 1;
            orderToAdd.ModifiedBy = 1;
            orderToAdd.IsDeleted = false;
            orderToAdd.CreatedDate = DateTime.UtcNow.Date;
            orderToAdd.ModifiedDate = DateTime.UtcNow.Date;

            await _orderRepository.AddAsync(orderToAdd);
        }

        public async Task UpdateAsync(int id, UpdateOrderDto order)
        {
            var orderToUpdate = await _orderRepository.GetOrderByIdAsync(id);
            if(orderToUpdate is null)
            {
                throw new ArgumentOutOfRangeException($"Order with Id {id} not found.");
            }
            orderToUpdate.ModifiedBy = 2;
            orderToUpdate.ModifiedDate = DateTime.UtcNow.Date;

            await _orderRepository.UpdateAsync(orderToUpdate);
        }

        public async Task DeleteAsync(int id)
        {
            var orderToDelete = await _orderRepository.GetOrderByIdAsync(id);
            if (orderToDelete is null)
            {
                throw new ArgumentOutOfRangeException($"Order with Id {id} not found.");
            }
            orderToDelete.ModifiedBy = 3;
            orderToDelete.ModifiedDate = DateTime.UtcNow.Date;
            orderToDelete.IsDeleted = true;

            await _orderRepository.DeleteAsync(orderToDelete);
        }

    }
}
