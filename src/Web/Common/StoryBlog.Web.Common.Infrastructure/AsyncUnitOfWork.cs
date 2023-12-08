using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Repositories;
using StoryBlog.Web.Common.Infrastructure.Repositories;
using System.Collections;

namespace StoryBlog.Web.Common.Infrastructure;

public sealed class AsyncUnitOfWork<T> : IAsyncUnitOfWork
    where T : class, IGenericDbContext
{
    private readonly Hashtable repositories;
    private T? context;
    private bool disposed;

    public AsyncUnitOfWork(T context)
    {
        this.context = context;
        repositories = new Hashtable();
    }

    public ValueTask DisposeAsync() => DisposeAsync(true);

    public IAsyncGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : IEntity
    {
        var entityType = typeof(TEntity);
        object? instance;

        if (repositories.ContainsKey(entityType))
        {
            instance = repositories[entityType];
        }
        else
        {
            var repositoryType = typeof(AsyncGenericRepository<,>).MakeGenericType(entityType, typeof(T));

            instance = Activator.CreateInstance(repositoryType, context);
            repositories.Add(entityType, instance);
        }

        if (instance is IAsyncGenericRepository<TEntity> repository)
        {
            return repository;
        }

        throw new Exception();
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        return context!.SaveChangesAsync(cancellationToken);
    }

    private async ValueTask DisposeAsync(bool dispose)
    {
        if (disposed)
        {
            return;
        }

        try
        {
            if (dispose)
            {
                /*if (context!.HasChanges())
                {
                    await context!.RollbackAsync();
                }*/

                context = null;
            }
        }
        finally
        {
            disposed = true;
        }
    }
}