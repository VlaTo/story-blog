namespace StoryBlog.Web.Microservices.Communication.WebApi.MessageBus.Consumers;

public class MessageConsumer
{
    public ILogger Logger
    {
        get;
    }

    protected MessageConsumer(ILogger logger)
    {
        Logger = logger;
    }
}