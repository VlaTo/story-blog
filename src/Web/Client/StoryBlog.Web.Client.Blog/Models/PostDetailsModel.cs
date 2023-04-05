using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Client.Blog.Models;

[DataContract]
public class PostDetailsModel
{
    [Required]
    [StringLength(255, ErrorMessage = "Length exceed")]
    public string Title
    {
        get;
        set;
    }

    [Required]
    [StringLength(255)]
    public string? Slug
    {
        get;
        set;
    }
}