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

    [JsonPropertyName(nameof(Text))]
    public string Text
    {
        get;
        set;
    }

    [JsonPropertyName(nameof(AuthorId))]
    public string? AuthorId
    {
        get;
        set;
    }

    [JsonPropertyName(nameof(PublicationStatus))]
    public CommentPublicationStatus PublicationStatus
    {
        get;
        set;
    }

    [JsonPropertyName(nameof(CreatedAt))]
    public DateTimeOffset CreatedAt
    {
        get;
        set;
    }
}