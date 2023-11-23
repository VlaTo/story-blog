using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public sealed class CreatePostRequest
{
    [Required]
    [StringLength(255)]
    public string Title
    {
        get;
        set;
    }

    [Required]
    [StringLength(255)]
    public string Slug
    {
        get;
        set;
    }

    [Required]
    [DataType(DataType.MultilineText)]
    [StringLength(4096)]
    public string Content
    {
        get;
        set;
    }
}