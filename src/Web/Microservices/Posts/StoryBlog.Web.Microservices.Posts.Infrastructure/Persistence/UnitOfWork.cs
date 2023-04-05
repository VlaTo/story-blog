using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Interfaces;
using StoryBlog.Web.Microservices.Posts.Domain.Repositories;
using System.Collections;

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
        var key = typeof(TEntity);
        object? instance;

        if (repositories.ContainsKey(key))
        {
            instance = repositories[key];
        }
        else
        {
            var repositoryType = typeof(GenericRepository<>).MakeGenericType(key);
            
            instance = Activator.CreateInstance(repositoryType, context);
            repositories.Add(key, instance);
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