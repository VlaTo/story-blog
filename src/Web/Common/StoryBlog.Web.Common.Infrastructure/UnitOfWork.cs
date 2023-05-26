using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Repositories;
using StoryBlog.Web.Common.Infrastructure.Repositories;
using System.Collections;
using StoryBlog.Web.Common.Application;

namespace StoryBlog.Web.Common.Infrastructure;

public sealed class UnitOfWork<T> : IUnitOfWork
    where T : class, IGenericDbContext
{
    private readonly Hashtable repositories;
    private T? context;
    private bool disposed;

    public UnitOfWork(T context)
    {
        this.context = context;
        repositories = new Hashtable();
    }

    public void Dispose()
    {
        ;
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
            var repositoryType = typeof(GenericRepository<,>).MakeGenericType(entityType, typeof(T));

            instance = Activator.CreateInstance(repositoryType, context);
            repositories.Add(entityType, instance);
        }

        if (instance is IGenericRepository<TEntity> repository)
        {
            return repository;
        }

        throw new Exception();
    }
    
    public void Commit()
    {
        context!.SaveChanges();
    }

    private void Dispose(bool dispose)
    {
        if (disposed)
        {
            return;
        }

        try
        {
            if (dispose)
            {
                if (context!.HasChanges())
                {
                    context!.Rollback();
                }

                context = null;
            }
        }
        finally
        {
            disposed = true;
        }
    }
}