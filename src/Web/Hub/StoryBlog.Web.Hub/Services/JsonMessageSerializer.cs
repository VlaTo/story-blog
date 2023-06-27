using System.Text;
using StoryBlog.Web.Hub.Common.Messages;
using StoryBlog.Web.Hub.Common.Services;

namespace StoryBlog.Web.Hub.Services;

internal sealed class JsonMessageSerializer : IMessageSerializer
{
    public byte[] Serialize<TMessage>(TMessage message) where TMessage : IHubMessage
    {
        var json = System.Text.Json.JsonSerializer.Serialize(message);
        return Encoding.UTF8.GetBytes(json);
    }

    public TMessage Deserialize<TMessage>(byte[] data) where TMessage : IHubMessage
    {
        var json = Encoding.UTF8.GetString(data);
        return System.Text.Json.JsonSerializer.Deserialize<TMessage>(json)!;
    }
}