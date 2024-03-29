namespace StoryBlog.Web.Microservices.Posts.WebApi.Configuration;

public class MessageBusOptions
{

    public bool PublishCreatedEvent
    {
        get;
        set;
    } = false;

    public bool PublishRemovedEvent
    {
        get;
        set;
    } = false;

    public bool PublishPostPostProcessedEvent
    {
        get;
        set;
    } = false;
}