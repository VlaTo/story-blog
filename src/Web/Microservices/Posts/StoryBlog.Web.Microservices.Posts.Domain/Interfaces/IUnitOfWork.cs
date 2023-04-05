using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Domain.Repositories;

namespace StoryBlog.Web.Microservices.Posts.Domain.Interfaces;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : IEntity;

    Task CommitAsync(CancellationToken cancellationToken = default);
}