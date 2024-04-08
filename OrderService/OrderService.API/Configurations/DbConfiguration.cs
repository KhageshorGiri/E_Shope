using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Data;

namespace OrderService.API.Configurations
{
    public static class DbConfiguration
    {
        public static void RegisterDbConfiguration(this WebApplicationBuilder builder)
        {
            // Add Db Configuration
            builder.Services.AddDbContext<OrderServiceDbContext>(option =>
                            option.UseNpgsql(builder.Configuration.GetConnectionString("PostgraceServerConnection")));

        }
    }
}
