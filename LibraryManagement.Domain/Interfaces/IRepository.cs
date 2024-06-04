using LibraryManagement.Domain.Primitives;
using LibraryManagement.Domain.Specifications;

namespace LibraryManagement.Domain.Interfaces;

public interface IRepository<TEntity, TId>
    where TId : notnull
    where TEntity : IAggregateRoot<TId>
{
    //Queries
    /// <summary>
    /// Get all the record of tables into a list of object
    /// </summary>
    Task<List<TEntity>> GetAllListAsync();

    /// <summary>
    /// Get all the record of tables and able to query with linq due to the IQueryable return
    /// Currently disable
    /// </summary>
    [Obsolete]
    IQueryable<TEntity> GetAll();

    /// <summary>
    /// Get the entity using its Id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity?> GetByIdAsync(TId id);

    //Insert
    /// <summary>
    /// Insert new entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> InsertAsync(TEntity entity);

    /// <summary>
    /// Remove entity from the database using its Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteByIdAsync(TId id);

    /// <summary>
    /// Count the number of record in the table
    /// </summary>
    /// <returns></returns>
    Task<int> CountAsync();

    /// <summary>
    /// Get all overloading that return the record of tables into a list of object supported by ISpecification
    /// </summary>
    /// <param name="spec"></param>
    /// <returns></returns>
    Task<List<TEntity>> GetAllListAsync(ISpecification<TEntity> spec);

    /// <summary>
    /// Count the number of record in the table supported by ISpecification
    /// </summary>
    /// <param name="spec"></param>
    /// <returns></returns>
    ValueTask<int> CountAsync(ISpecification<TEntity> spec);

    /// <summary>
    /// Get the entity using its Id  supported by ISpecification
    /// </summary>
    /// <param name="spec"></param>
    /// <returns></returns>
    Task<TEntity?> GetByIdAsync(ISpecification<TEntity> spec);
}