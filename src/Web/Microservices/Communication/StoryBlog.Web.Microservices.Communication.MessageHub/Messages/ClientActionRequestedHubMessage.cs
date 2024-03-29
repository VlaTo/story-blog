using StoryBlog.Web.MessageHub.Messages;

namespace StoryBlog.Web.Microservices.Communication.MessageHub.Messages;

public sealed class ClientActionRequestedHubMessage : IHubMessage
{
    public required string Action
    {
        get; 
        set;
    }
}