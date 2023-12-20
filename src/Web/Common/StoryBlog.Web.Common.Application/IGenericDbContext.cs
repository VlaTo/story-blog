namespace StoryBlog.Web.Common.Application;

public interface IGenericDbContext
{
    bool HasChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
}