namespace StoryBlog.Web.Common.Application;

public interface IGenericDbContext
{
    Task<int> CommitAsync(CancellationToken cancellationToken = default);

    Task RollbackAsync(CancellationToken cancellationToken = default);
}