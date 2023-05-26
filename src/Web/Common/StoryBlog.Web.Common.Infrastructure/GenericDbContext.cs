using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Common.Application;

namespace StoryBlog.Web.Common.Infrastructure;

public abstract class GenericDbContext : DbContext, IGenericDbContext
{
    protected GenericDbContext()
    {
    }

    protected GenericDbContext(DbContextOptions options)
        : base(options)
    {
    }

    /*public virtual async Task<int> CommitAsync(CancellationToken cancellationToken = default)
    {
        if (ChangeTracker.HasChanges())
        {
            return await SaveChangesAsync(true, cancellationToken);
        }

        return 0;
    }*/

    public virtual bool HasChanges() => ChangeTracker.HasChanges();

    public abstract Task RollbackAsync(CancellationToken cancellationToken = default);

    public virtual void Rollback()
    {
        ;
    }
}