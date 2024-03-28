namespace StoryBlog.Web.Microservices.Posts.WebApi.Configuration;

public class MessageBusOptions
{

    public bool PublishCreatedEvent
    {
        get;
        set;
    }

    public bool PublishRemovedEvent
    {
        get;
        set;
    }

    public string HubChannelName
    {
        get;
        set;
    }

}