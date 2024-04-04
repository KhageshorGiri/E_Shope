using Microsoft.EntityFrameworkCore;
using OrderService.Infrastructure.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Loger configuration
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);

    logger.Information("Starting web application");

    // Add services to the container.

    // Add Db Configuration
    builder.Services.AddDbContext<OrderServiceDbContext>(option =>
                    option.UseNpgsql(builder.Configuration.GetConnectionString("PostgraceServerConnection")));

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}

catch (Exception ex)
{
    logger.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
