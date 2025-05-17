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
using System.Reflection;
using System.Text.Json;

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
                throw new InvalidOperationException("CORS policy name is not configured (CorsSettings:PolicyName).");

            if (origins == null || origins.Length == 0)
                throw new InvalidOperationException("CORS origins are not configured (CorsSettings:Origins).");

            if (methods == null || methods.Length == 0)
                throw new InvalidOperationException("CORS methods are not configured (CorsSettings:Methods).");

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
            throw new InvalidOperationException("Rate limit endpoint is not configured (RateLimit:Endpoint).");

        if (string.IsNullOrWhiteSpace(period))
            throw new InvalidOperationException("Rate limit period is not configured (RateLimit:Period).");

        if (!double.TryParse(limitStr, out var limit))
            throw new InvalidOperationException("Rate limit 'Limit' is not a valid number (RateLimit:Limit).");

        if (!int.TryParse(httpStatusCodeStr, out var httpStatusCode))
            throw new InvalidOperationException("Rate limit 'HttpStatusCode' is not a valid integer (RateLimit:HttpStatusCode).");

        if (string.IsNullOrWhiteSpace(realIpHeader))
            throw new InvalidOperationException("Rate limit 'RealIpHeader' is not configured (RateLimit:RealIpHeader).");

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
            throw new InvalidOperationException("Connection string 'MeetingScheduleAppConnection' is missing or empty.");

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
                throw new InvalidOperationException("Connection string 'MeetingScheduleAppConnection' not found.");

            options.UseSqlServer(connectionString);
        });
    }
}
