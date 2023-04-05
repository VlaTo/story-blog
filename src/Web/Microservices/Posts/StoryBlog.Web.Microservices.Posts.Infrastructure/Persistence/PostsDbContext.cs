using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Microservices.Posts.Application.Contexts;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;
using StoryBlog.Web.Microservices.Posts.Infrastructure.Configuration;

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Persistence;

public sealed class PostsDbContext : DbContext, IPostsDbContext
{
    public DbSet<Post> Posts => Set<Post>();
    
    public PostsDbContext()
    {
    }

    public PostsDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PostEntityConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}