﻿using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain.Specifications;

namespace StoryBlog.Web.Common.Domain.Repositories;

/*public interface IGenericRepository<TEntity> : IDisposable where TEntity : IEntity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task<TEntity[]> QueryAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<TEntity?> FindAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);

    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    
    Task SaveAsync(TEntity entity, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}*/

public interface IAsyncGenericRepository<TEntity> : IAsyncDisposable where TEntity : IEntity
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    Task<TEntity[]> QueryAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    
    Task<TEntity?> FindAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    
    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task<int> RemoveAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}