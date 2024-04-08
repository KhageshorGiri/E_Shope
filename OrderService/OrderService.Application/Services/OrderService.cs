using Microsoft.Extensions.Logging;
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
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(IOrderRepository orderRepository, IEventConsumer eventConsumer, ILogger<OrdersService> logger)
        {
            _orderRepository = orderRepository;
            _eventConsumer = eventConsumer;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("Request Recived and Forwarding to Repository");
            var allOrders = await _orderRepository.GetAllAsync(cancellationToken);
            _logger.LogInformation("Order Service Called successfully for {0} service method", nameof(GetAllAsync));
            return allOrders.Select(order => order.ToOrderDto());
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("Request Recived and Forwarding to Repository");
            var order = await _orderRepository.GetOrderByIdAsync(id, cancellationToken);
            _logger.LogInformation("Order Service Called successfully for {0} service method", nameof(GetAllAsync));
            return order?.ToOrderDto();
        }

        public async Task AddAsync(CreateOrderDto order, CancellationToken cancellationToken = default(CancellationToken))
        {
            var orderToAdd = order.ToOrder();
            orderToAdd.CreatedBy = 1;
            orderToAdd.ModifiedBy = 1;
            orderToAdd.IsDeleted = false;
            orderToAdd.CreatedDate = DateTime.UtcNow.Date;
            orderToAdd.ModifiedDate = DateTime.UtcNow.Date;

            _logger.LogInformation("Request Recived and Forwarding to Repository");
            await _orderRepository.AddAsync(orderToAdd, cancellationToken);
            _logger.LogInformation("Order Service Called successfully for {0} service method with parmeters {1}", nameof(AddAsync), order.ToString());
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

            _logger.LogInformation("Request Recived and Forwarding to Repository");
            await _orderRepository.UpdateAsync(orderToUpdate, cancellationToken);
            _logger.LogInformation("Order Service Called successfully for {0} service method with parmeters {1}", nameof(UpdateAsync), orderToUpdate.ToString());
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("Request Recived and Forwarding to Repository");
            var orderToDelete = await _orderRepository.GetOrderByIdAsync(id, cancellationToken);
            if (orderToDelete is null)
            {
                throw new ArgumentOutOfRangeException($"Order with Id {id} not found.");
            }
            orderToDelete.ModifiedBy = 3;
            orderToDelete.ModifiedDate = DateTime.UtcNow.Date;
            orderToDelete.IsDeleted = true;

            _logger.LogInformation("Request Recived and Forwarding to Repository");
            await _orderRepository.DeleteAsync(orderToDelete, cancellationToken);
            _logger.LogInformation("Order Service Called successfully for {0} service method with parmeters {1}", nameof(DeleteAsync), orderToDelete.ToString());
        }

    }
}
