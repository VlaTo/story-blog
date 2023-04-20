using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using System.Collections;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Repositories;
using StoryBlog.Web.Common.Infrastructure.Repositories;

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly Hashtable repositories;
    private readonly PostsDbContext context;

    public UnitOfWork(PostsDbContext context)
    {
        this.context = context;
        repositories = new Hashtable();
    }

    public ValueTask DisposeAsync()
    {
        if (context.ChangeTracker.HasChanges())
        {
            ;
        }
        
        return ValueTask.CompletedTask;
    }

    public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : IEntity
    {
        var entityType = typeof(TEntity);
        object? instance;

        if (repositories.ContainsKey(entityType))
        {
            instance = repositories[entityType];
        }
        else
        {
            var repositoryType = typeof(GenericRepository<,>).MakeGenericType(entityType, typeof(PostsDbContext));
            
            instance = Activator.CreateInstance(repositoryType, context);
            repositories.Add(entityType, instance);
        }

        if (instance is IGenericRepository<TEntity> repository)
        {
            return repository;
        }

        throw new Exception();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return context.SaveChangesAsync(cancellationToken);
    }
}