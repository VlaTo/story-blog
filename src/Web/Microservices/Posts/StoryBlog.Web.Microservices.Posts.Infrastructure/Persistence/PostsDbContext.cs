using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Common.Infrastructure;
using StoryBlog.Web.Microservices.Posts.Application.Contexts;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Infrastructure.Configuration;

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Persistence;

public sealed class PostsDbContext : GenericDbContext, IPostsDbContext
{
    public DbSet<Post> Posts => Set<Post>();

    public DbSet<PostProcessTask> PostProcessTasks => Set<PostProcessTask>();

    public PostsDbContext()
    {
    }

    public PostsDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public override Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PostEntityConfiguration());
        modelBuilder.ApplyConfiguration(new PostProcessTaskEntityConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}