using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using ProductsBackend.Models;
using ProductsBackend.Repositories;

namespace ProductsBackend.Endpoints;

// PUBLIC_INTERFACE
public static class ProductEndpoints
{
    /// <summary>
    /// Maps product CRUD endpoints.
    /// </summary>
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products")
            .WithTags("Products");

        // GET /products
        group.MapGet("/", async ([FromServices] IProductRepository repo, CancellationToken ct) =>
            {
                var items = await repo.GetAllAsync(ct);
                return Results.Ok(items);
            })
            .WithName("GetProducts")
            .WithSummary("List products")
            .WithDescription("Returns all products.")
            .Produces<IReadOnlyList<Product>>(StatusCodes.Status200OK);

        // GET /products/{id}
        group.MapGet("/{id:guid}", async ([FromServices] IProductRepository repo, Guid id, CancellationToken ct) =>
            {
                var item = await repo.GetByIdAsync(id, ct);
                return item is not null ? Results.Ok(item) : Results.NotFound();
            })
            .WithName("GetProductById")
            .WithSummary("Get a product by ID")
            .WithDescription("Returns a single product by its unique identifier.")
            .Produces<Product>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        // POST /products
        group.MapPost("/", async ([FromServices] IProductRepository repo, [FromBody] CreateProductDto dto, CancellationToken ct) =>
            {
                // Model validation is handled by [ApiController] conventions when using controllers.
                // For minimal APIs, we can manually validate or rely on data annotations via TryValidate.
                // Here, we perform basic server-side guards:
                if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price < 0 || dto.Quantity < 0)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        ["name"] = string.IsNullOrWhiteSpace(dto.Name) ? new[] { "Name is required." } : Array.Empty<string>(),
                        ["price"] = dto.Price < 0 ? new[] { "Price must be >= 0." } : Array.Empty<string>(),
                        ["quantity"] = dto.Quantity < 0 ? new[] { "Quantity must be >= 0." } : Array.Empty<string>(),
                    });
                }

                var entity = new Product
                {
                    Name = dto.Name.Trim(),
                    Price = dto.Price,
                    Quantity = dto.Quantity
                };

                var created = await repo.CreateAsync(entity, ct);
                return Results.Created($"/products/{created.Id}", created);
            })
            .WithName("CreateProduct")
            .WithSummary("Create a new product")
            .WithDescription("Creates a new product with the provided data and returns the created resource.")
            .Produces<Product>(StatusCodes.Status201Created)
            .ProducesValidationProblem();

        // PUT /products/{id}
        group.MapPut("/{id:guid}", async ([FromServices] IProductRepository repo, Guid id, [FromBody] UpdateProductDto dto, CancellationToken ct) =>
            {
                if (string.IsNullOrWhiteSpace(dto.Name) || dto.Price < 0 || dto.Quantity < 0)
                {
                    return Results.ValidationProblem(new Dictionary<string, string[]>
                    {
                        ["name"] = string.IsNullOrWhiteSpace(dto.Name) ? new[] { "Name is required." } : Array.Empty<string>(),
                        ["price"] = dto.Price < 0 ? new[] { "Price must be >= 0." } : Array.Empty<string>(),
                        ["quantity"] = dto.Quantity < 0 ? new[] { "Quantity must be >= 0." } : Array.Empty<string>(),
                    });
                }

                var entity = new Product
                {
                    Name = dto.Name.Trim(),
                    Price = dto.Price,
                    Quantity = dto.Quantity
                };

                var updated = await repo.UpdateAsync(id, entity, ct);
                return updated is null ? Results.NotFound() : Results.Ok(updated);
            })
            .WithName("UpdateProduct")
            .WithSummary("Update an existing product")
            .WithDescription("Updates a product by ID with the provided data.")
            .Produces<Product>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem();

        // DELETE /products/{id}
        group.MapDelete("/{id:guid}", async ([FromServices] IProductRepository repo, Guid id, CancellationToken ct) =>
            {
                var removed = await repo.DeleteAsync(id, ct);
                return removed ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteProduct")
            .WithSummary("Delete a product")
            .WithDescription("Deletes a product by its ID.")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
