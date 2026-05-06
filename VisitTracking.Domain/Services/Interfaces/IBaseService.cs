using System.Linq.Expressions;

namespace VisitTracking.Domain.Services.Interfaces;

/// <summary>
/// Enterprise generic service contract used by application services.
/// Keeps validation and orchestration logic aligned across new Guid/auditable
/// entities and the existing int-key scaffolded entities.
/// </summary>
/// <typeparam name="TEntity">Entity type.</typeparam>
/// <typeparam name="TKey">Primary key type.</typeparam>
public interface IBaseService<TEntity, TKey>
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