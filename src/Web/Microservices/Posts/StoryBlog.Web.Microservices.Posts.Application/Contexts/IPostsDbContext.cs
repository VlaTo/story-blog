using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Contexts;

public interface IPostsDbContext : IAsyncDisposable, IGenericDbContext
{
    DbSet<Post> Posts
    {
        get;
    }
}