using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VisitTracking.Domain.Entities;
using VisitTracking.Domain.RepositoryInterfaces;
using VisitTracking.Infrastructure.Data;

namespace VisitTracking.Infrastructure.Repositories.Common;

/// <summary>
/// Shared EF Core repository base that supports read filtering, eager loading,
/// validation-ready key handling, and audit stamping for BaseEntity-derived models.
/// Works with the existing int-key scaffolded entities.
/// </summary>
public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey>
    where TEntity : class
    where TKey : notnull
{
    private readonly AppDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public BaseRepository(AppDbContext dbContext)
    {
        ArgumentNullException.ThrowIfNull(dbContext);

        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();
    }

    public async Task<List<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();

        if (include is not null)
        {
            query = include(query);
        }

        if (predicate is not null)
        {
            query = query.Where(predicate);
        }

        return await query.ToListAsync().ConfigureAwait(false);
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        GuardKey(id);
        return await _dbSet.FindAsync(new object?[] { id }).ConfigureAwait(false);
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        StampForCreate(entity);

        await _dbSet.AddAsync(entity).ConfigureAwait(false);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return entity;
    }

    public async Task<TEntity?> UpdateAsync(TKey id, TEntity entity)
    {
        GuardKey(id);
        ArgumentNullException.ThrowIfNull(entity);

        var existingEntity = await _dbSet.FindAsync(new object?[] { id }).ConfigureAwait(false);
        if (existingEntity is null)
        {
            return null;
        }

        var existingCreatedAt = existingEntity is BaseEntity auditableExisting
            ? auditableExisting.CreatedAt
            : (DateTime?)null;

        StampForUpdate(entity);
        _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

        if (existingEntity is BaseEntity auditableEntity && existingCreatedAt.HasValue)
        {
            auditableEntity.CreatedAt = existingCreatedAt.Value;
            auditableEntity.Touch();
        }

        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return existingEntity;
    }

    public async Task<bool> DeleteAsync(TKey id)
    {
        GuardKey(id);

        var existingEntity = await _dbSet.FindAsync(new object?[] { id }).ConfigureAwait(false);
        if (existingEntity is null)
        {
            return false;
        }

        _dbSet.Remove(existingEntity);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);

        return true;
    }

    public async Task<bool> ExistsAsync(TKey id)
    {
        GuardKey(id);
        return await _dbSet.FindAsync(new object?[] { id }).ConfigureAwait(false) is not null;
    }

    private static void GuardKey(TKey id)
    {
        if (EqualityComparer<TKey>.Default.Equals(id, default!))
        {
            throw new ArgumentException("The entity key cannot be the default value.", nameof(id));
        }
    }

    private static void StampForCreate(TEntity entity)
    {
        if (entity is not BaseEntity auditableEntity)
        {
            return;
        }

        var utcNow = DateTime.UtcNow;
        auditableEntity.CreatedAt = utcNow;
        auditableEntity.UpdatedAt = utcNow;
    }

    private static void StampForUpdate(TEntity entity)
    {
        if (entity is BaseEntity auditableEntity)
        {
            auditableEntity.Touch();
        }
    }
}
