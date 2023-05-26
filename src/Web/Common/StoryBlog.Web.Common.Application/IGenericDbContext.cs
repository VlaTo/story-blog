namespace StoryBlog.Web.Common.Application;

public interface IGenericDbContext
{
    bool HasChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    
    int SaveChanges();

    Task RollbackAsync(CancellationToken cancellationToken = default);

    void Rollback();
}