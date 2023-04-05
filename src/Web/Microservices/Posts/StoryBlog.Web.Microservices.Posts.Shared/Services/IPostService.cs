using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Posts.Shared.Services;

public interface IPostService
{
    Task<PostModel?> GetPost(Guid key);
}