using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public sealed class CreatePostRequest
{
    [Required]
    [StringLength(1024)]
    public string Title
    {
        get;
        set;
    }

    [Required]
    public string Slug
    {
        get;
        set;
    }

    [Required]
    [DataType(DataType.Html)]
    public string Content
    {
        get;
        set;
    }
}