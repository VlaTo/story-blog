using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Application.Contexts;

public interface ICommentsDbContext : IAsyncDisposable, IGenericDbContext
{
    DbSet<Comment> Comments
    {
        get;
    }
}