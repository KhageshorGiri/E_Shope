using OrderService.API.Configurations;
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

    //Add services to the container.
    builder.RegisterDbConfiguration();
    builder.ServicesPilelineConfiguration();

    // Moddleware Configuration
    var app = builder.Build();
    app.MiddleWarePipelIne();
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
