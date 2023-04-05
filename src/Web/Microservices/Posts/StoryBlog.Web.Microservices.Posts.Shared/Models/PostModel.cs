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
    [JsonPropertyName(nameof(Status))]
    public PostModelStatus Status
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(CreatedAt))]
    public DateTime CreatedAt
    {
        get;
        set;
    }
}