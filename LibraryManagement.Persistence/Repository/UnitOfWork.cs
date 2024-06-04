using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Persistence.Entity_Framework_Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Persistence.Repository;

internal sealed class UnitOfWork : IUnitOfWork
{
    private readonly ILogger<UnitOfWork> _logger;
    private readonly AppDbContext _appDbContext;

    public UnitOfWork(ILogger<UnitOfWork> logger, AppDbContext appDbContext)
    {
        _logger = logger;
        _appDbContext = appDbContext;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities();
        
        _logger.LogDebug("On save changes...");
        return await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditableEntities()
    {
        IEnumerable<EntityEntry<IAuditableEntity>> entries =
            _appDbContext.ChangeTracker.Entries<IAuditableEntity>();

        foreach (EntityEntry<IAuditableEntity> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(e => e.CreatedAt).CurrentValue = DateTime.Now;
                entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.Now;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(e => e.UpdatedAt).CurrentValue = DateTime.Now;
            }
        }
    }
}