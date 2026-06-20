using Demo.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Persistence.Data;

public sealed class UnitOfWork(IApplicationDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}
