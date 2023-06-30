namespace StoryBlog.Web.MessageHub.Server.Configuration;

public sealed class MessageHubMessage
{
    public Type MessageType
    {
        get;
    }

    public List<Type> Handlers
    {
        get;
    }

    public MessageHubMessage(Type messageType)
    {
        MessageType = messageType;
        Handlers = new List<Type>();
    }
}