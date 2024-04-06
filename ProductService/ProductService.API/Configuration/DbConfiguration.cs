using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.DataContext;

namespace ProductService.API.Configuration
{
    public static class DbConfiguration
    {
        public static void RegisterDbConfiguration(this WebApplicationBuilder builder)
        {
            // Add DbContext Service
            builder.Services.AddDbContext<ProductServiceDbContext>(option =>
                            option.UseNpgsql(builder.Configuration.GetConnectionString("PostgraceSqlServerConnection")));

        }
    }
}
