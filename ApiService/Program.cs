using AspirePostgresRag.ApiService;
using AspirePostgresRag.ApiService.Endpoints;
using Scalar.AspNetCore;
using ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(o =>
{
    o.AddSchemaTransformer<ExampleSchemaTransformer>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (true) // || app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.Map("/", () => Results.Redirect("/scalar/v1"))
        .WithName("RootRedirect")
        .WithSummary("Redirect to Scalar API");
}

string[] summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.CacheOutput()
.WithName("GetWeatherForecast");

app.MapHelloWorldEndpoints();

app.MapDefaultEndpoints();

app.Run();
