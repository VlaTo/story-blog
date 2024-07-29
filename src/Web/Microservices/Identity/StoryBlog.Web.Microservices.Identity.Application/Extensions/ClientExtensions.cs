using StoryBlog.Web.Common.Identity.Permission;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

public static class ClientExtensions
{
    /// <summary>
    /// Returns true if the client is an implicit-only client.
    /// </summary>
    public static bool IsImplicitOnly(this Storage.Client? client)
    {
        return client is { AllowedGrantTypes.Count: 1 } && GrantType.Implicit == client.AllowedGrantTypes.First();
    }


}