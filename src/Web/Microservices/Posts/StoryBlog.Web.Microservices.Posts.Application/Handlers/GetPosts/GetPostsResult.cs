using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPosts;

public sealed class GetPostsResult : AbstractResult
{
    public IReadOnlyList<Brief>? Posts
    {
        get;
    }

    public GetPostsResult(IReadOnlyList<Brief>? posts)
    {
        Posts = posts;
    }

    public override bool IsSuccess()
    {
        return null != Posts;
    }
}