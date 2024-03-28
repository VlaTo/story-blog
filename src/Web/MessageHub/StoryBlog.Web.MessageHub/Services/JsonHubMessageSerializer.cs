using StoryBlog.Web.MessageHub.Messages;
using System.Text;
using System.Text.Json;

namespace StoryBlog.Web.MessageHub.Services;

/// <summary>
/// 
/// </summary>
public sealed class JsonHubMessageSerializer : IHubMessageSerializer
{
    /// <inheritdoc cref="IHubMessageSerializer.Serialize{TMessage}" />
    public byte[] Serialize<TMessage>(TMessage message) where TMessage : IHubMessage
    {
        var json = JsonSerializer.Serialize(message);
        Console.WriteLine($"serialized json: {json}");
        return Encoding.UTF8.GetBytes(json);
    }

    /// <inheritdoc cref="IHubMessageSerializer.Deserialize" />
    public IHubMessage Deserialize(Type messageType, ArraySegment<byte> data)
    {
        var json = Encoding.UTF8.GetString(data);
        Console.WriteLine($"json to deserialize: {json}");
        return (IHubMessage)JsonSerializer.Deserialize(json, messageType)!;
    }
}