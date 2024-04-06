using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.IRepositories;
using ProductService.Infrastructure.DataContext;

namespace ProductService.Infrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductServiceDbContext _dbContext;

        public ProductRepository(ProductServiceDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Products.Where(x => x.Id == id && x.IsDeleted == false)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(cancellationToken);
                                         
        }

        public async Task<IEnumerable<Product>?> GetAllAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _dbContext.Products.Where(x=>x.IsDeleted == false).AsNoTracking()
                                          .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default(CancellationToken))
        {
            using(var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    _dbContext.Products.Add(product);
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

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    _dbContext.Products.Update(product);
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

        public async Task DeleteAsync(Product product, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    _dbContext.Products.Update(product);
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
