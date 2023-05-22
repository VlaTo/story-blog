using Microsoft.IdentityModel.Tokens;

namespace StoryBlog.Web.Microservices.Identity.Application.Storage;

/// <summary>
/// Information about a security key
/// </summary>
public class SecurityKeyInfo
{
    /// <summary>
    /// The key
    /// </summary>
    public SecurityKey Key
    {
        get;
        set;
    }

    /// <summary>
    /// The signing algorithm
    /// </summary>
    public string SigningAlgorithm
    {
        get;
        set;
    }
}