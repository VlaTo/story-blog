using StoryBlog.Web.MessageHub.Messages;

namespace StoryBlog.Web.Microservices.Communication.MessageHub.Messages;

public sealed class NewPostPublishedHubMessage : IHubMessage
{
    public required Guid PostKey
    {
        get;
        set;
    }

    public required DateTimeOffset CreatedAt
    {
        get;
        set;
    }

    public required string Slug
    {
        get;
        set;
    }
    
    public required string AuthorId
    {
        get;
        set;
    }
}