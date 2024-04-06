using ProductService.Application.Messaging;
using ProductService.Application.ServiceInterfaces;
using ProductService.Application.Services;
using ProductService.Domain.IRepositories;
using ProductService.Infrastructure.Repository;

namespace ProductService.API.Configuration
{
    public static class HttpServicesPilelineConfiguration
    {
        public static void ServicesPilelineConfiguration(this WebApplicationBuilder builder)
        {
            RegisterServicesConfiguration(builder);
            RegisterServices(builder);
        }

        private static void RegisterServicesConfiguration(WebApplicationBuilder builder)
        {
            // Build in services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //health checks
            builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("PostgraceSqlServerConnection"));

            //Exception Handeler
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        }

        private static void RegisterServices(WebApplicationBuilder builder) 
        {

            // Product Service and repository
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IProductService, ProductsService>();

            // Kafka Service
            builder.Services.AddSingleton<IEventPublisher, KafkaEventPublisher>();

        }
    }
}
