using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Identity.Application.Core;

/// <summary>
/// 
/// </summary>
public class ClaimLite
{
    public string Type
    {
        get;
        set;
    }

    public string Value
    {
        get;
        set;
    }

    public string? ValueType
    {
        get;
        set;
    }
}

/// <summary>
/// 
/// </summary>
public class ClaimConverter : JsonConverter<Claim>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override Claim Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var source = JsonSerializer.Deserialize<ClaimLite>(ref reader, options);
        var target = new Claim(source.Type, source.Value, source.ValueType);

        return target;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, Claim value, JsonSerializerOptions options)
    {
        var target = new ClaimLite
        {
            Type = value.Type,
            Value = value.Value,
            ValueType = value.ValueType
        };

        if (target.ValueType == ClaimValueTypes.String)
        {
            target.ValueType = null;
        }

        JsonSerializer.Serialize(writer, target, options);
    }
}