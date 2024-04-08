using OrderService.Application.Dtos;
using OrderService.Application.MappingProfile;
using OrderService.Application.Messaging;
using OrderService.Application.ServiceInterfaces;
using OrderService.Domain.IRepositories;

namespace OrderService.Application.Services
{
    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEventConsumer _eventConsumer;

        public OrdersService(IOrderRepository orderRepository, IEventConsumer eventConsumer)
        {
            _orderRepository = orderRepository;
            _eventConsumer = eventConsumer;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var allOrders = await _orderRepository.GetAllAsync(cancellationToken);

            return allOrders.Select(order => order.ToOrderDto());
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var order = await _orderRepository.GetOrderByIdAsync(id, cancellationToken);
            if (order is null)
            {
                throw new ArgumentOutOfRangeException($"Order with Id {id} not found.");
            }
            return order.ToOrderDto();
        }

        public async Task AddAsync(CreateOrderDto order, CancellationToken cancellationToken = default(CancellationToken))
        {
            var orderToAdd = order.ToOrder();
            orderToAdd.CreatedBy = 1;
            orderToAdd.ModifiedBy = 1;
            orderToAdd.IsDeleted = false;
            orderToAdd.CreatedDate = DateTime.UtcNow.Date;
            orderToAdd.ModifiedDate = DateTime.UtcNow.Date;

            await _orderRepository.AddAsync(orderToAdd, cancellationToken);
        }

        public async Task UpdateAsync(int id, UpdateOrderDto order, CancellationToken cancellationToken = default(CancellationToken))
        {
            var orderToUpdate = await _orderRepository.GetOrderByIdAsync(id, cancellationToken);
            if(orderToUpdate is null)
            {
                throw new ArgumentOutOfRangeException($"Order with Id {id} not found.");
            }
            orderToUpdate.ModifiedBy = 2;
            orderToUpdate.ModifiedDate = DateTime.UtcNow.Date;

            await _orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var orderToDelete = await _orderRepository.GetOrderByIdAsync(id, cancellationToken);
            if (orderToDelete is null)
            {
                throw new ArgumentOutOfRangeException($"Order with Id {id} not found.");
            }
            orderToDelete.ModifiedBy = 3;
            orderToDelete.ModifiedDate = DateTime.UtcNow.Date;
            orderToDelete.IsDeleted = true;

            await _orderRepository.DeleteAsync(orderToDelete, cancellationToken);
        }

    }
}
