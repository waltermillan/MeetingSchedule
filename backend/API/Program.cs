using API.Extensions;
using AspNetCoreRateLimit;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Filters;

var builder = WebApplication.CreateBuilder(args);

var policyName = builder.Configuration["CorsSettings:PolicyName"] ?? "CorsPolicy";

// Logging configuration
builder.ConfigureLogging();

// Rate limiting configuration
builder.Services.ConfigureRateLimiting(builder.Configuration);

// Force automatically generated paths (as with [controller]) to be lowercase
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Add services to the container.
builder.Services.ConfigureCors(builder.Configuration);
builder.Services.AddAplicacionServices();
builder.Services.AddControllers();

// Configure DbContext with SQL Server
builder.Services.ConfigureDatabaseContext(builder.Configuration);

// Configure health checks
builder.Services.ConfigureHealthCheck(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Uses custom exception middleware
app.UseHandlerException();

app.UseIpRateLimiting();
app.UseStatusCodePagesWithReExecute("/errors/{0}");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure Migrations
await app.Services.ConfigureMigrationsAsync();

app.UseCors(policyName);
app.UseHttpsRedirection();
app.UseAuthorization();

// Health check mapping with custom JSON response
builder.Services.HealthCheckMapping(app);

app.MapControllers();

app.Run();

