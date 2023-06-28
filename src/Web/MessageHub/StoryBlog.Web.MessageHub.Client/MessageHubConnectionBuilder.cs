using StoryBlog.Web.MessageHub.Services;

namespace StoryBlog.Web.MessageHub.Client;

public class MessageHubConnectionBuilder
{
    private string? url;

    public MessageHubConnectionBuilder()
    {
    }

    public MessageHubConnectionBuilder WithUrl(string value)
    {
        url = value;

        return this;
    }

    public MessageHubConnection Build()
    {
        var serializer = new JsonHubMessageSerializer();
        return new MessageHubConnection(new Uri(url!), serializer);
    }
}