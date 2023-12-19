using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public sealed class PagingInfo
{
    [DataMember]
    [JsonPropertyName(nameof(Next))]
    public string? Next
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(Previous))]
    public string? Previous
    {
        get;
        set;
    }
}