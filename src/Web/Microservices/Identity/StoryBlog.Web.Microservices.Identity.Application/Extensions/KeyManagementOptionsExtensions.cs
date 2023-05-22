using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;

namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

internal static class KeyManagementOptionsExtensions
{
    public static bool IsWithinInitializationDuration(this KeyManagementOptions options, TimeSpan age)
    {
        return (age <= options.InitializationDuration);
    }

    public static bool IsRetired(this KeyManagementOptions options, TimeSpan age)
    {
        return (age >= options.KeyRetirementAge);
    }

    public static bool IsExpired(this KeyManagementOptions options, TimeSpan age)
    {
        return (age >= options.RotationInterval);
    }
}