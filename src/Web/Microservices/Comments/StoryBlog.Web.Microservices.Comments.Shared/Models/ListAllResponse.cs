using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Comments.Shared.Models;

[DataContract]
public sealed class ListAllResponse
{
    [DataMember]
    [JsonPropertyName(nameof(Comments))]
    public IReadOnlyList<CommentModel> Comments
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
        Comments = Array.Empty<CommentModel>();
    }
}