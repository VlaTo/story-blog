using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Comments.Shared.Models;

[DataContract]
public sealed class CommentModel : CommentDetailsModel
{
    [JsonPropertyName(nameof(Comments))]
    public IReadOnlyList<CommentModel> Comments
    {
        get;
        set;
    }
}