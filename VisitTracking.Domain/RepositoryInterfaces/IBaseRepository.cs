using System.Linq.Expressions;

namespace VisitTracking.Domain.RepositoryInterfaces;

/// <summary>
/// Enterprise generic repository contract for EF-backed persistence.
/// Works with both new Guid/auditable entities and existing int-key scaffolded entities.
/// </summary>
/// <typeparam name="TEntity">Entity type.</typeparam>
/// <typeparam name="TKey">Primary key type.</typeparam>
public interface IBaseRepository<TEntity, TKey>
    where TEntity : class
    where TKey : notnull
{
    Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);

    Task<TEntity?> GetByIdAsync(TKey id);

    Task<TEntity> CreateAsync(TEntity entity);

    Task<TEntity?> UpdateAsync(TKey id, TEntity entity);

    Task<bool> DeleteAsync(TKey id);

    Task<bool> ExistsAsync(TKey id);
}