namespace StoryBlog.Web.Microservices.Identity.Application.Extensions;

internal static class DictionaryExtensions
{
    public static bool IsNullOrEmpty(this Dictionary<string, object>? discovery)
    {
        return null == discovery || 0 == discovery.Count;
    }
}