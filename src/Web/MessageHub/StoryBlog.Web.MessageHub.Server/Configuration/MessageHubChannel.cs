namespace StoryBlog.Web.MessageHub.Server.Configuration;

public sealed class MessageHubChannel
{
    public string Name
    {
        get;
    }

    public List<MessageHubMessage> Messages
    {
        get;
    }

    public MessageHubChannel(string name, List<MessageHubMessage> messages)
    {
        Name = name;
        Messages = messages;
    }
}