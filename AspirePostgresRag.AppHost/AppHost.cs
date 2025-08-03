var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

builder.AddProject<Projects.AspirePostgresRag_ApiService>("apiservice")
    .WithHttpHealthCheck("/health")
    .WithReference(cache).WaitFor(cache)
    ;

builder.Build().Run();
