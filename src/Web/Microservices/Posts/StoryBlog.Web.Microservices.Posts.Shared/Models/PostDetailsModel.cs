using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Posts.Shared.Models;

[DataContract]
public class PostDetailsModel
{
    [DataMember]
    [JsonPropertyName(nameof(Title))]
    public string Title
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(Slug))]
    public string Slug
    {
        get;
        set;
    }

    [DataMember]
    [JsonPropertyName(nameof(Text))]
    public string Text
    {
        get;
        set;
    }
}