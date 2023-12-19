using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public sealed class PostPublicityResponse
{
    [DataMember]
    [JsonPropertyName(nameof(PostKey))]
    public Guid PostKey
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
}