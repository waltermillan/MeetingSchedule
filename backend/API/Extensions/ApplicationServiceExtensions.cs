using API.Middlewares;
using API.Services;
using AspNetCoreRateLimit;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Logging;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog.Filters;
using Serilog;
using System.Reflection;
using System.Text.Json;
using Core.Constants;

namespace API.Extensions;
public static class ApplicationServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            var policyName = configuration["CorsSettings:PolicyName"];
            var origins = configuration.GetSection("CorsSettings:Origins").Get<string[]>();
            var methods = configuration.GetSection("CorsSettings:Methods").Get<string[]>();

            if (string.IsNullOrWhiteSpace(policyName))
                throw new InvalidOperationException(ConfigurationMessages.CORSPolicyNameNotConfigured);

            if (origins == null || origins.Length == 0)
                throw new InvalidOperationException(ConfigurationMessages.CORSOriginsNotConfigured);

            if (methods == null || methods.Length == 0)
                throw new InvalidOperationException(ConfigurationMessages.CORSMethodsNotConfigured);

            options.AddPolicy(policyName, builder =>
                builder.WithOrigins(origins)
                       .WithMethods(methods)
                       .AllowAnyHeader());
        });
    }

    public static void AddAplicacionServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddAutoMapper(Assembly.GetEntryAssembly());
        services.AddScoped<IContactRepository, ContactRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IContactTagRepository, ContactTagRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<ContactTagService>();
        services.AddScoped<UserService>();
        services.AddSingleton<ILoggingService, SerilogLoggingService>();
    }

    public static void ConfigureRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        var endpoint = configuration["RateLimit:Endpoint"];
        var period = configuration["RateLimit:Period"];
        var limitStr = configuration["RateLimit:Limit"];
        var httpStatusCodeStr = configuration["RateLimit:HttpStatusCode"];
        var realIpHeader = configuration["RateLimit:RealIpHeader"];

        if (string.IsNullOrWhiteSpace(endpoint))
            throw new InvalidOperationException(ConfigurationMessages.RateLimitEndpointNotConfigured);

        if (string.IsNullOrWhiteSpace(period))
            throw new InvalidOperationException(ConfigurationMessages.RateLimitPeriodNotConfigured);

        if (!double.TryParse(limitStr, out var limit))
            throw new InvalidOperationException(ConfigurationMessages.RateLimitLimitNotValidNumber);

        if (!int.TryParse(httpStatusCodeStr, out var httpStatusCode))
            throw new InvalidOperationException(ConfigurationMessages.RateLimitHttpStatusCodeNotValidInteger);

        if (string.IsNullOrWhiteSpace(realIpHeader))
            throw new InvalidOperationException(ConfigurationMessages.RateLimitRealIpHeaderNotConfigured);

        services.AddMemoryCache();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();

        services.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = httpStatusCode;
            options.RealIpHeader = realIpHeader;
            options.GeneralRules =
        [
            new() {
                Endpoint = endpoint,
                Period = period,
                Limit = limit
            }
        ];
        });
    }

    public static IApplicationBuilder UseHandlerException(this IApplicationBuilder app)
    {
        return app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static void ConfigureHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MeetingScheduleAppConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException(ConfigurationMessages.ConnectionStringMissingOrEmpty);

        services.AddHealthChecks()
            .AddSqlServer(
                connectionString,
                name: "sql",
                failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
                tags: ["db", "sql"]);
    }

    public static void HealthCheckMapping(this IServiceCollection _, WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";

                // Serializing the health check status in JSON format
                var result = JsonSerializer.Serialize(new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(entry => new
                    {
                        name = entry.Key,
                        status = entry.Value.Status.ToString(),
                        error = entry.Value.Exception?.Message,
                        duration = entry.Value.Duration.TotalMilliseconds + "ms"
                    })
                });

                await context.Response.WriteAsync(result);
            }
        });
    }

    public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
        {
            var connectionString = configuration.GetConnectionString("MeetingScheduleAppConnection");

            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException(ConfigurationMessages.ConnectionStringNotFound);

            options.UseSqlServer(connectionString);
        });
    }

    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        var appName = builder.Configuration["SystemName:Name"] ?? "MeetingSchedule";

        builder.Logging.ClearProviders();
        builder.Logging.AddFilter("Microsoft", LogLevel.Warning);
        builder.Logging.AddFilter("System", LogLevel.Warning);

        // Serilog Configuration
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File($"logs/{appName}-.log", rollingInterval: RollingInterval.Day)
            .Filter.ByExcluding(Matching.FromSource("Microsoft.EntityFrameworkCore"))
            .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore"))
            .CreateLogger();

        builder.Logging.AddSerilog();
    }

    public static async Task ConfigureMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var loggerFactory = scopedServices.GetRequiredService<ILoggerFactory>();

        try
        {
            var context = scopedServices.GetRequiredService<Context>();
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = loggerFactory.CreateLogger("MigrationLogger");
            logger.LogError(ex, ConfigurationMessages.ErrorDuringMigration);
        }
    }
}
