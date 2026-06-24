using Demo.Domain.Entities;
using Demo.Domain.Repositories;
using Demo.Infrastructure.Persistence.Data;
using Demo.Infrastructure.Persistence.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Persistence.Repositories;

public sealed class ProductRepository(IApplicationDbContext context)
    : BaseRepository<Product>(context), IProductRepository
{
    public async Task<Product?> GetByPublicIdAsync(Guid publicId, CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking().FirstOrDefaultAsync(product => product.PublicId == publicId, cancellationToken);

    public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default) =>
        await DbSet.AnyAsync(product => product.Name == name, cancellationToken);

    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(product, cancellationToken);
        return product;
    }

    public async Task<IReadOnlyList<Product>> ListAsync(CancellationToken cancellationToken = default) =>
        await DbSet.AsNoTracking()
            .OrderBy(product => product.Name)
            .ToListAsync(cancellationToken);
}
