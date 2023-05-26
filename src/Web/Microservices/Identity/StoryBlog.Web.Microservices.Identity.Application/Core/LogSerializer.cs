using System.Text.Json.Serialization;
using System.Text.Json;

namespace StoryBlog.Web.Microservices.Identity.Application.Core;

/// <summary>
/// Helper to JSON serialize object data for logging.
/// </summary>
internal static class LogSerializer
{
    public static readonly JsonSerializerOptions Options;

    static LogSerializer()
    {
        Options = new()
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
        //Options.Converters.Add(new JsonStringEnumConverter());
    }

    /// <summary>
    /// Serializes the specified object.
    /// </summary>
    /// <param name="logObject">The object.</param>
    /// <returns></returns>
    public static string Serialize(object logObject) => JsonSerializer.Serialize(logObject, Options);
}