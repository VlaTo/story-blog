﻿using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Repositories;
using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Common.Infrastructure.Repositories;

public class GenericRepository<TEntity, TDbContext> : IGenericRepository<TEntity>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    private readonly TDbContext context;

    public GenericRepository(TDbContext context)
    {
        this.context = context;
    }

    #region IGenericRepository

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var entities = context.Set<TEntity>();
        await entities.AddAsync(entity, cancellationToken);
    }

    public Task<TEntity[]> QueryAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var entities = context.Set<TEntity>().AsQueryable();
        var query = SpecificationEvaluator<TEntity>.Query(entities, specification);
        return query.ToArrayAsync(cancellationToken);
    }

    public Task<TEntity?> FindAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var entities = context.Set<TEntity>().AsQueryable();
        var query = SpecificationEvaluator<TEntity>.Query(entities, specification);
        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var entities = context.Set<TEntity>().AsQueryable();
        var query = SpecificationEvaluator<TEntity>.Query(entities, specification);
        return 0 < await query.CountAsync(cancellationToken);
    }

    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var entities = context.Set<TEntity>();
        
        entities.Remove(entity);

        return Task.CompletedTask;
    }

    public Task SaveAsync(TEntity entity, CancellationToken cancellationToken)
    {
        var entities = context.Set<TEntity>();

        entities.Update(entity);

        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
        //return context.DisposeAsync();
    }

    #endregion
}