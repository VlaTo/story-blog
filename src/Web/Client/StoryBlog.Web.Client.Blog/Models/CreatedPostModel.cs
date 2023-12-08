using System.Runtime.Serialization;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Client.Blog.Models;

[DataContract]
public sealed class CreatedPostModel : PostDetailsModel
{
    public PostModelStatus Status
    {
        get;
        set;
    }

    public DateTimeOffset CreatedAt
    {
        get;
        set;
    }

    public Uri? Location
    {
        get;
        set;
    }
}