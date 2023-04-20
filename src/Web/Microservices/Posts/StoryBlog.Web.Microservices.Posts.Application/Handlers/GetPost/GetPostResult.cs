using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;

public class GetPostResult : AbstractResult
{
    public Post? Post
    {
        get;
    }

    public GetPostResult(Post? post)
    {
        Post = post;
    }

    public override bool IsSuccess()
    {
        return null != Post;
    }
}