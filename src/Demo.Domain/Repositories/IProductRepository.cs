using Demo.Domain.Entities;

namespace Demo.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default);
    Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default);
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Product>> ListAsync(CancellationToken cancellationToken = default);
}
