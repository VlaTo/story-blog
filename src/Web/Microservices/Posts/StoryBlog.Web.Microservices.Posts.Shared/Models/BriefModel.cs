using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public class BriefModel : PostDetailsModel
{
    [DataMember]
    [JsonPropertyName(nameof(Key))]
    public Guid Key
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(Status))]
    public PostModelStatus Status
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(AllowedActions))]
    public AllowedActions AllowedActions
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(IsPublic))]
    public bool IsPublic
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