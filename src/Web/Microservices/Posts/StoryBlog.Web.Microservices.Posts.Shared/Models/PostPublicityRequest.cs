using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public sealed class PostPublicityRequest
{
    [Required]
    public bool IsPublic
    {
        get;
        set;
    }
}