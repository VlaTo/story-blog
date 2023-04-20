using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Application.Contexts;

public interface ICommentsDbContext : IAsyncDisposable
{
    public DbSet<Comment> Comments
    {
        get;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}