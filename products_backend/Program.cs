using ProductsBackend.Endpoints;
using ProductsBackend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();

// OpenAPI/Swagger via NSwag with app-level metadata
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "Products Management API";
    settings.Description = "A modern, minimal REST API to manage products with CRUD operations.";
    settings.Version = "v1";
    settings.DocumentName = "v1";
    // Tags are inferred from endpoint metadata (WithTags on endpoints).
});

// Register repository (in-memory for now; can be swapped with DB-backed implementation)
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
    config.DocumentTitle = "Products API Docs";
});

// Health check endpoint
// PUBLIC_INTERFACE
app.MapGet("/", () => new { message = "Healthy" })
   .WithName("HealthCheck")
   .WithSummary("Health check")
   .WithDescription("Returns a simple healthy status response.")
   .WithTags("System");

// Map product endpoints
app.MapProductEndpoints();

app.Run();