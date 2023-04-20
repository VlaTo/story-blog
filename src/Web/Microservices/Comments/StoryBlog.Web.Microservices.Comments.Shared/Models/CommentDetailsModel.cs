using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Comments.Shared.Models;

[DataContract]
public class CommentDetailsModel
{
    [JsonPropertyName(nameof(Key))]
    public Guid Key
    {
        get;
        set;
    }

    [JsonPropertyName(nameof(Text))]
    public string Text
    {
        get;
        set;
    }
}