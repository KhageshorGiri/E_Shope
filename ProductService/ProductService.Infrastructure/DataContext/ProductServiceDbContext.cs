using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;

namespace ProductService.Infrastructure.DataContext
{
    public class ProductServiceDbContext : DbContext
    {
        public ProductServiceDbContext(DbContextOptions<ProductServiceDbContext> options)
            :base(options)
        {
            
        }

        #region Dbset

        public DbSet<Product> Products { get; set; }

        #endregion Dbset
    }
}
