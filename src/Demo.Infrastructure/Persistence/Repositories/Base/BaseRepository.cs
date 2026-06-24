using Demo.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Persistence.Repositories.Base;

public abstract class BaseRepository<TEntity>(IApplicationDbContext context) where TEntity : class
{
    protected IApplicationDbContext Context { get; } = context;
    protected DbSet<TEntity> DbSet => Context.GetEntitySet<TEntity>();
}
