using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Microservices.Comments.Shared.Models;

[DataContract]
public sealed class CreateCommentRequest
{
    [Required]
    public Guid PostKey
    {
        get;
        set;
    }

    public Guid? ParentKey
    {
        get;
        set;
    }

    [Required]
    [StringLength(255)]
    public string Text
    {
        get;
        set;
    }
}