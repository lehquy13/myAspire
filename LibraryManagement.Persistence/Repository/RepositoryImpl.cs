using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Domain.Primitives;
using LibraryManagement.Domain.Specifications;
using LibraryManagement.Persistence.Entity_Framework_Core;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.Repository;

public class RepositoryImpl<TEntity, TId> : IRepository<TEntity, TId>
    where TEntity : Entity<TId>, IAggregateRoot<TId>
    where TId : notnull
{
    protected readonly AppDbContext AppDbContext;
    protected readonly IAppLogger<RepositoryImpl<TEntity, TId>> Logger;
    protected readonly string ErrorMessage = "{Message} with exception: {Ex}";

    public RepositoryImpl(AppDbContext appDbContext, IAppLogger<RepositoryImpl<TEntity, TId>> logger)
    {
        AppDbContext = appDbContext;
        this.Logger = logger;
    }

    public async Task<bool> DeleteByIdAsync(TId id)
    {
        try
        {
            var deleteRecord = await AppDbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (deleteRecord == null)
                return false;

            AppDbContext.Set<TEntity>().Remove(deleteRecord);
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetAllListAsync", ex.Message);
            return new();
        }
    }

    public async Task<int> CountAsync()
    {
        try
        {
            return await AppDbContext.Set<TEntity>().CountAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetAllListAsync", ex.Message);
            return 0;
        }
    }

    public async ValueTask<int> CountAsync(ISpecification<TEntity> spec)
    {
        var specificationResult = GetQuery(AppDbContext.Set<TEntity>(), spec, true);

        return await specificationResult.CountAsync();
    }

    public async Task<TEntity?> GetByIdAsync(ISpecification<TEntity> spec)
    {
        var specificationResult = GetQuery(AppDbContext.Set<TEntity>(), spec);

        return await specificationResult.FirstOrDefaultAsync();
    }

    public async Task<List<TEntity>> GetAllListAsync(ISpecification<TEntity> spec)
    {
        var specificationResult = GetQuery(AppDbContext.Set<TEntity>(), spec);

        return await specificationResult.AsNoTracking().ToListAsync();
    }

    public virtual async Task<List<TEntity>> GetAllListAsync()
    {
        try
        {
            return await AppDbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in GetAllListAsync", ex.Message);
            Logger.LogError(ErrorMessage, "GetAllListAsync", ex.Message);
            return new();
        }
    }

    public IQueryable<TEntity> GetAll()
    {
        try
        {
            return AppDbContext.Set<TEntity>().AsQueryable<TEntity>();
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in GetAll", ex.Message);
            throw;
        }
    }

    public virtual async Task<TEntity?> GetByIdAsync(TId id)
    {
        try
        {
            return await AppDbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in GetByIdAsync", ex.Message);
            return null;
        }
    }

    public async Task<bool> InsertAsync(TEntity entity)
    {
        try
        {
            await AppDbContext.Set<TEntity>().AddAsync(entity);
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in InsertAsync", ex.Message);
            return false;
        }
    }

    protected static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> specification, bool isForCount = false)

    {
        var query = inputQuery;

        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        query = specification.IncludeExpressions.Aggregate(query, (current, include) => current.Include(include));

        //Handle then include
        query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

        if (specification.IsPagingEnabled && !isForCount)
        {
            query = query.Skip(specification.Skip - 1)
                .Take(specification.Take);
        }

        return query;
    }
}