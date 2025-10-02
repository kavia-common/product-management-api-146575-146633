using ProductsBackend.Models;

namespace ProductsBackend.Repositories;

// PUBLIC_INTERFACE
public interface IProductRepository
{
    /// <summary>
    /// Returns all products.
    /// </summary>
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// Returns a product by id or null if not found.
    /// </summary>
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default);

    /// <summary>
    /// Creates a new product and returns it.
    /// </summary>
    Task<Product> CreateAsync(Product product, CancellationToken ct = default);

    /// <summary>
    /// Updates an existing product (by id) or returns null if not found.
    /// </summary>
    Task<Product?> UpdateAsync(Guid id, Product product, CancellationToken ct = default);

    /// <summary>
    /// Deletes a product by id and returns whether it existed.
    /// </summary>
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
