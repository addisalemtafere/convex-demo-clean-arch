using Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Persistence.Data;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<TEntity> GetEntitySet<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
