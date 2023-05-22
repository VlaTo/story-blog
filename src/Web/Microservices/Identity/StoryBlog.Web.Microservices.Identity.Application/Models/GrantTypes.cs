using IdentityModel;

namespace StoryBlog.Web.Microservices.Identity.Application.Models;

/// <summary>
/// 
/// </summary>
public static class GrantTypes
{
    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> Implicit => new[]
    {
        GrantType.Implicit
    };

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> ImplicitAndClientCredentials => new[]
    {
        GrantType.Implicit,
        GrantType.ClientCredentials
    };

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> Code => new[]
    {
        GrantType.AuthorizationCode
    };

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> CodeAndClientCredentials => new[]
    {
        GrantType.AuthorizationCode,
        GrantType.ClientCredentials
    };

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> Hybrid => new[]
    {
        GrantType.Hybrid
    };

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> HybridAndClientCredentials => new[]
    {
        GrantType.Hybrid,
        GrantType.ClientCredentials
    };

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> ClientCredentials => new[]
    {
        GrantType.ClientCredentials
    };

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> ResourceOwnerPassword => new[]
    {
        GrantType.ResourceOwnerPassword
    };

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> ResourceOwnerPasswordAndClientCredentials => new[]
    {
        GrantType.ResourceOwnerPassword,
        GrantType.ClientCredentials
    };

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> DeviceFlow => new[]
    {
        GrantType.DeviceFlow
    };

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> Ciba => new[]
    {
        OidcConstants.GrantTypes.Ciba
    };
}