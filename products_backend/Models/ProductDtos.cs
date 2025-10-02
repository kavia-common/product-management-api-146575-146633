using System.ComponentModel.DataAnnotations;

namespace ProductsBackend.Models;

// PUBLIC_INTERFACE
public sealed class CreateProductDto
{
    /// <summary>
    /// Name of the product.
    /// </summary>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Price of the product. Must be >= 0.
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity of the product. Must be >= 0.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}

// PUBLIC_INTERFACE
public sealed class UpdateProductDto
{
    /// <summary>
    /// Name of the product.
    /// </summary>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Price of the product. Must be >= 0.
    /// </summary>
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity of the product. Must be >= 0.
    /// </summary>
    [Range(0, int.MaxValue)]
    public int Quantity { get; set; }
}
