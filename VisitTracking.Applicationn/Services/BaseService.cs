using System.Linq.Expressions;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Domain.Services.Interfaces;

namespace VisitTracking.Application.Services;

/// <summary>
/// Shared application service base that validates inputs before delegating to the repository.
/// </summary>
public abstract class BaseService<TEntity, TKey> : IBaseService<TEntity, TKey>
    where TEntity : class
    where TKey : notnull
{
    protected readonly IBaseRepository<TEntity, TKey> Repository;

    protected BaseService(IBaseRepository<TEntity, TKey> repository)
    {
        ArgumentNullException.ThrowIfNull(repository);

        Repository = repository;
    }

    public virtual Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
        => Repository.GetAllAsync(predicate, include);

    public virtual Task<TEntity?> GetByIdAsync(TKey id)
    {
        GuardKey(id);
        return Repository.GetByIdAsync(id);
    }

    public virtual Task<TEntity> CreateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return Repository.CreateAsync(entity);
    }

    public virtual Task<TEntity?> UpdateAsync(TKey id, TEntity entity)
    {
        GuardKey(id);
        ArgumentNullException.ThrowIfNull(entity);

        return Repository.UpdateAsync(id, entity);
    }

    public virtual Task<bool> DeleteAsync(TKey id)
    {
        GuardKey(id);
        return Repository.DeleteAsync(id);
    }

    public virtual Task<bool> ExistsAsync(TKey id)
    {
        GuardKey(id);
        return Repository.ExistsAsync(id);
    }

    private static void GuardKey(TKey id)
    {
        if (EqualityComparer<TKey>.Default.Equals(id, default!))
        {
            throw new ArgumentException("The entity key cannot be the default value.", nameof(id));
        }
    }
}