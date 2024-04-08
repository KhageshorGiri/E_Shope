using APIGateway;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using System.Net.Mime;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Loger configuration
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    // Configuring Serilog
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);

    logger.Information("Starting web application");

    // Add exception handler
    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    
    // Add Health Check
    builder.Services.AddHealthChecks();

    // Add Ocelot configuration
    builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
    builder.Services.AddOcelot(builder.Configuration);

    // Middleware PipeLine
    var app = builder.Build();

    app.UseRouting();

    app.UseStatusCodePages();
    app.UseExceptionHandler();


    // Health check Middleware configuration
    app.MapHealthChecks("/gatewayhealth", new HealthCheckOptions()
    {
        ResponseWriter = async (context, report) =>
        {
            var result = JsonSerializer.Serialize(
                new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key.ToString(),
                        status = entry.Value.Status.ToString(),
                        exception = entry.Value.Exception is not null ? entry.Value.Exception.Message : "none",
                        duration = entry.Value.Duration.ToString()
                    })
                });

            context.Response.ContentType = MediaTypeNames.Application.Json;
            await context.Response.WriteAsync(result);
        }
    });

    app.UseEndpoints(e =>
    {
        e.MapControllers();
    });
    app.UseOcelot().Wait();

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

