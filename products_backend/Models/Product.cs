using System.ComponentModel.DataAnnotations;

namespace ProductsBackend.Models;

/// <summary>
/// Represents a product in the system.
/// </summary>
public sealed class Product
{
    /// <summary>
    /// Unique identifier of the product.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Name of the product.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Price of the product.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Available quantity of the product.
    /// </summary>
    public int Quantity { get; set; }
}
