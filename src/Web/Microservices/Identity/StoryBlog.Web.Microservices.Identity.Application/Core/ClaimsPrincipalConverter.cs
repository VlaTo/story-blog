using IdentityModel;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Microservices.Identity.Application.Core;

/// <summary>
/// 
/// </summary>
public class ClaimsPrincipalLite
{
    public string AuthenticationType
    {
        get;
        set;
    }

    public ClaimLite[] Claims
    {
        get;
        set;
    }
}

/// <summary>
/// 
/// </summary>
public class ClaimsPrincipalConverter : JsonConverter<ClaimsPrincipal>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override ClaimsPrincipal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var source = JsonSerializer.Deserialize<ClaimsPrincipalLite>(ref reader, options);
        return ToClaimsPrincipal(source);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, ClaimsPrincipal value, JsonSerializerOptions options)
    {
        var target = ToClaimsPrincipalLite(value);
        JsonSerializer.Serialize(writer, target, options);
    }

    private static ClaimsPrincipal ToClaimsPrincipal(ClaimsPrincipalLite principal)
    {
        var claims = principal.Claims.Select(x => new Claim(x.Type, x.Value, x.ValueType ?? ClaimValueTypes.String)).ToArray();
        var id = new ClaimsIdentity(claims, principal.AuthenticationType, JwtClaimTypes.Name, JwtClaimTypes.Role);

        return new ClaimsPrincipal(id);
    }

    private static ClaimsPrincipalLite ToClaimsPrincipalLite(ClaimsPrincipal principal)
    {
        var claims = principal.Claims
            .Select(x => new ClaimLite
            {
                Type = x.Type,
                Value = x.Value,
                ValueType = x.ValueType == ClaimValueTypes.String ? null : x.ValueType
            })
            .ToArray();

        return new ClaimsPrincipalLite
        {
            AuthenticationType = principal.Identity!.AuthenticationType!,
            Claims = claims
        };
    }
}