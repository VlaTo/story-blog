namespace StoryBlog.Web.Client.Blog.Core;

/// <summary>
/// 
/// </summary>
internal static class Stub
{
    public static Action Nop => () => { };

    public static Func<bool> True => () => true;
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
internal static class Stub<T>
{
    public static Action<T> Nop => _ => { };

    public static Func<T, bool> True => _ => true;
}