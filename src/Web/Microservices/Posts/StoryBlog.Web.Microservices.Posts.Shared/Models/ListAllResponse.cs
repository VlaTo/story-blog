using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public sealed class ListAllResponse
{
    [DataMember]
    [JsonPropertyName(nameof(Posts))]
    public IReadOnlyCollection<BriefModel> Posts
    {
        get; 
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(PageNumber))]
    public int PageNumber
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(PageSize))]
    public int PageSize
    {
        get;
        set;
    }

    public ListAllResponse()
    {
        Posts = Array.Empty<BriefModel>();
    }
}