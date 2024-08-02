using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Identity;

namespace StoryBlog.Web.Microservices.Identity.Application.Models;

/// <summary>
/// 
/// </summary>
public static class GrantTypes
{
    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> Implicit =>
    [
        GrantType.Implicit
    ];

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> ImplicitAndClientCredentials =>
    [
        GrantType.Implicit,
        GrantType.ClientCredentials
    ];

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> Code =>
    [
        GrantType.AuthorizationCode
    ];

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> CodeAndClientCredentials =>
    [
        GrantType.AuthorizationCode,
        GrantType.ClientCredentials
    ];

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> Hybrid =>
    [
        GrantType.Hybrid
    ];

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> HybridAndClientCredentials =>
    [
        GrantType.Hybrid,
        GrantType.ClientCredentials
    ];

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> ClientCredentials =>
    [
        GrantType.ClientCredentials
    ];

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> ResourceOwnerPassword =>
    [
        GrantType.ResourceOwnerPassword
    ];

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> ResourceOwnerPasswordAndClientCredentials =>
    [
        GrantType.ResourceOwnerPassword,
        GrantType.ClientCredentials
    ];

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> DeviceFlow =>
    [
        GrantType.DeviceFlow
    ];

    /// <summary>
    /// 
    /// </summary>
    public static ICollection<string> Ciba =>
    [
        OidcConstants.GrantTypes.Ciba
    ];
}