using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Repositories;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Persistence;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IEntity
{
    private readonly PostsDbContext context;

    public GenericRepository(PostsDbContext context)
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

    public ValueTask DisposeAsync()
    {
        //return context.DisposeAsync();
        return ValueTask.CompletedTask;
    }
}