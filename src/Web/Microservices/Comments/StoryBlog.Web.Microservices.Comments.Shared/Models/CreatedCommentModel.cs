using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Comments.Shared.Models;

[DataContract]
public sealed class CreatedCommentModel : CommentDetailsModel
{
    [JsonPropertyName(nameof(PostKey))]
    public Guid PostKey
    {
        get;
        set;
    }

    [JsonPropertyName(nameof(ParentKey))]
    public Guid? ParentKey
    {
        get;
        set;
    }
}