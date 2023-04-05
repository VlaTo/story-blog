using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Comments.Shared.Models;

[DataContract]
public sealed class CommentsListResponse
{
    [DataMember]
    [JsonPropertyName(nameof(Comments))]
    public IReadOnlyCollection<CommentModel> Comments
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

    public CommentsListResponse()
    {
        Comments = Array.Empty<CommentModel>();
    }
}