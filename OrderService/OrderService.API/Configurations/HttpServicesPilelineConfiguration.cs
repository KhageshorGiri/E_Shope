using OrderService.Application.Messaging;
using OrderService.Application.ServiceInterfaces;
using OrderService.Application.Services;
using OrderService.Domain.IRepositories;
using OrderService.Infrastructure.Repositories;

namespace OrderService.API.Configurations
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
                .AddNpgSql(builder.Configuration.GetConnectionString("PostgraceServerConnection"));

            //Exception Handeler
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        }

        private static void RegisterServices(WebApplicationBuilder builder)
        {

            // Registering services
            builder.Services.AddScoped<IOrderService, OrdersService>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();

            // Kafka service
            builder.Services.AddSingleton<IEventConsumer, KafkaEventConsumer>();
        }
    }
}
