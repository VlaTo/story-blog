namespace StoryBlog.Web.MessageHub.Server.Helpers;

internal static class StringHelper
{
    public static bool Equals(string actual, params string[] expected) => expected.Any(x => String.Equals(x, actual));
}