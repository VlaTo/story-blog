using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Microservices.Comments.Application.Contexts;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;
using StoryBlog.Web.Microservices.Comments.Infrastructure.Configuration;

namespace StoryBlog.Web.Microservices.Comments.Infrastructure.Persistence;

public sealed class CommentsDbContext : DbContext, ICommentsDbContext
{
    public DbSet<Comment> Comments => Set<Comment>();

    public CommentsDbContext()
    {
    }

    public CommentsDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CommentEntityConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}