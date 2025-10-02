using System.Collections.Concurrent;
using ProductsBackend.Models;

namespace ProductsBackend.Repositories;

/// <summary>
/// Thread-safe, in-memory repository for products. Intended for demo and testing.
/// </summary>
public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _store = new();

    public InMemoryProductRepository()
    {
        // Seed a couple of products for quick testing
        var p1 = new Product { Name = "Ocean Notebook", Price = 9.99m, Quantity = 120 };
        var p2 = new Product { Name = "Amber Pen", Price = 2.49m, Quantity = 500 };
        _store.TryAdd(p1.Id, p1);
        _store.TryAdd(p2.Id, p2);
    }

    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken ct = default)
    {
        var list = _store.Values.OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase).ToList().AsReadOnly();
        return Task.FromResult<IReadOnlyList<Product>>(list);
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        _store.TryGetValue(id, out var product);
        return Task.FromResult(product);
    }

    public Task<Product> CreateAsync(Product product, CancellationToken ct = default)
    {
        // Ensure unique Id
        if (product.Id == Guid.Empty)
        {
            product.Id = Guid.NewGuid();
        }
        _store[product.Id] = product;
        return Task.FromResult(product);
    }

    public Task<Product?> UpdateAsync(Guid id, Product product, CancellationToken ct = default)
    {
        if (!_store.ContainsKey(id))
        {
            return Task.FromResult<Product?>(null);
        }

        product.Id = id;
        _store[id] = product;
        return Task.FromResult<Product?>(product);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var removed = _store.TryRemove(id, out _);
        return Task.FromResult(removed);
    }
}
