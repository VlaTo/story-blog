using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Comments.Application.Services;

public interface IPostsApiClient
{
    Task<PostModel> GetPostAsync(Guid post, CancellationToken cancellationToken);
}