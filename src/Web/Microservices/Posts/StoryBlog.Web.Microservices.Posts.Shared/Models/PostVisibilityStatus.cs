using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public enum PostVisibilityStatus
{
    Public,
    ForAuthenticated,
    Private
}