using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Repositories;

namespace StoryBlog.Web.Common.Domain;

public interface IUnitOfWork : IAsyncDisposable
{
    IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : IEntity;

    Task CommitAsync(CancellationToken cancellationToken = default);
}