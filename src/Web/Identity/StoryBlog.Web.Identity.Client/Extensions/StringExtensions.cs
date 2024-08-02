namespace StoryBlog.Web.Identity.Client.Extensions;

internal static class StringExtensions
{
    public const char Slash = '/';

    public static string? RemoveTrailingSlash(this string? url) => 
        null != url && url.EndsWith(Slash) ? url.Substring(0, url.Length - 1) : url;

    public static string? EnsureTrailingSlash(this string? url) =>
        null != url && false == url.EndsWith(Slash) ? (url + Slash) : url;
}