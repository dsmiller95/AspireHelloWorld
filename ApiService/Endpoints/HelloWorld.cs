namespace AspirePostgresRag.ApiService.Endpoints;

public static class HelloWorld
{
    public static void MapHelloWorldEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/hello", (HelloRequest request) => request.Greeting + " " + request.Name)
            .WithName("Hello world")
            .WithSummary("Returns a greeting message based on the provided name and greeting.");
    }
    
    record HelloRequest(string Name, Greeting Greeting) : IHaveExample
    {
        public static object GetExample() => new HelloRequest("World", Greeting.Hello);
    }

    enum Greeting
    {
        Hello = 1,
        Hi = 2,
        Greetings = 3,
    }
}
