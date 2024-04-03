using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Domain.IRepositories;
using ProductService.Infrastructure.DataContext;

namespace ProductService.Infrastructure.Repository
{
    public class ProductRepository : IProductRepositorycs
    {
        private readonly ProductServiceDbContext _dbContext;

        public ProductRepository(ProductServiceDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _dbContext.Products.Where(x => x.Id == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
                                         
        }

        public async Task<IEnumerable<Product>?> GetAllAsync()
        {
            return await _dbContext.Products.AsNoTracking()
                                          .ToListAsync();
        }

        public async Task AddAsync(Product product)
        {
            using(var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Products.Add(product);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task UpdateAsync(Product product)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Products.Update(product);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task DeleteAsync(Product product)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.Products.Remove(product);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

    }
}
