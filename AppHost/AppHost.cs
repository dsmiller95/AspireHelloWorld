using Scalar.Aspire;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(cache).WaitFor(cache)
    ;

// Add Scalar API Reference for all services
var scalar = builder.AddScalarApiReference(options =>
{
    // Configure global options. They will apply to all services
    options.WithTheme(ScalarTheme.Purple);
});

// Configure API References for specific services
scalar
    .WithApiReference(apiService, options => options.WithOpenApiRoutePattern("/openapi/{documentName}.json"));

builder.Build().Run();

