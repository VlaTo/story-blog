using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Contexts;

public interface IPostsDbContext : IAsyncDisposable
{
    public DbSet<Post> Posts
    {
        get;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}