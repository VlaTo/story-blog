using System.Text.Json.Serialization;
using System.Text.Json;

namespace StoryBlog.Web.Microservices.Identity.Application.Core;

public static class ObjectSerializer
{
    private static readonly JsonSerializerOptions Options = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public static string ToString(object o)
    {
        return JsonSerializer.Serialize(o, Options);
    }

    public static T? FromString<T>(string value)
    {
        return JsonSerializer.Deserialize<T>(value, Options);
    }
}