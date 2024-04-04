using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Data
{
    public class OrderServiceDbContext : DbContext
    {
        public OrderServiceDbContext(DbContextOptions<OrderServiceDbContext> options)
            :base(options)
        {
            
        }

        #region DbSets

        public DbSet<Order> Orders { get; set; }

        #endregion DbSets
    }
}
