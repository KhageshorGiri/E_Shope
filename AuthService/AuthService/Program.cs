using AuthService;
using AuthService.DataContext;
using AuthService.Dtos;
using AuthService.Models;
using AuthService.Services;
using AuthService.Services.IService;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    // Configure serilog logger globally.
    builder.Logging.ClearProviders();
    builder.Logging.AddSerilog(logger);

    logger.Information("Starting web application");

    // Add services to the container.

    builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("ApiSettings:JwtOptions"));

    builder.Services.AddDbContext<AuthServiceDbContext>(option =>
    {
        option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthServiceDbContext>()
        .AddDefaultTokenProviders();

    // Add Health check
    builder.Services.AddHealthChecks()
        .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")); ;

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Exception Handeler
    builder.Services.AddProblemDetails();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

    builder.Services.AddJwtTokenValidator(builder.Configuration.GetSection("ApiSettings:JwtOptions:Secret").Value);

    builder.Services.AddScoped<IJwtTokenProvider, JswTokenProvider>();
    builder.Services.AddScoped<IAuthService, AuthsService>();


    var app = builder.Build();

    app.UseStatusCodePages();
    app.UseExceptionHandler();


    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    // Health check Middleware configuration
    app.MapHealthChecks("/authservicehealth", new HealthCheckOptions()
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

