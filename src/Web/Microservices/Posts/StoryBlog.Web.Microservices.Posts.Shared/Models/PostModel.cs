using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public class PostModel : PostDetailsModel
{
    [DataMember]
    [JsonPropertyName(nameof(Key))]
    public Guid Key
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(PublicationStatus))]
    public PostPublicationStatus PublicationStatus
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(CommentsCount))]
    public long CommentsCount
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(CreatedAt))]
    public DateTimeOffset CreatedAt
    {
        get;
        set;
    }
}