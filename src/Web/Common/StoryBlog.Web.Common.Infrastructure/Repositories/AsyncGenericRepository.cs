using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Repositories;
using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Common.Infrastructure.Repositories;

public class AsyncGenericRepository<TEntity, TDbContext> : IAsyncGenericRepository<TEntity>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    private readonly TDbContext context;

    public AsyncGenericRepository(TDbContext context)
    {
        this.context = context;
    }

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

    public Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var entities = context.Set<TEntity>().AsQueryable();
        var query = SpecificationEvaluator<TEntity>.Query(entities, specification);
        return query.CountAsync(cancellationToken);
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

    public Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        var collection = context.Set<TEntity>();
        collection.Remove(entity);
        return Task.CompletedTask;
    }

    public Task<int> RemoveAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var entities = context.Set<TEntity>().AsQueryable();
        var query = SpecificationEvaluator<TEntity>.Query(entities, specification);
        return query.ExecuteDeleteAsync(cancellationToken);
    }

    public Task RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        var collection = context.Set<TEntity>();
        collection.RemoveRange(entities);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        //return context.DisposeAsync();
        return ValueTask.CompletedTask;
    }
}