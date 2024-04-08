using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.IRepositories;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderServiceDbContext _dbContext;

        public OrderRepository(OrderServiceDbContext context)
        {
            _dbContext = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Orders.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Order?> GetOrderByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Orders.Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
        }

        public async  Task AddAsync(Order order, CancellationToken cancellationToken = default(CancellationToken))
        {
            using(var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    _dbContext.Orders.Add(order);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    _dbContext.Orders.Update(order);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        public async Task DeleteAsync(Order order, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    _dbContext.Orders.Update(order);
                    await _dbContext.SaveChangesAsync(cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}
