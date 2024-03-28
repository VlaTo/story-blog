namespace StoryBlog.Web.Client.Core;

/// <summary>
/// 
/// </summary>
public static class Stub
{
    public static Action Nop => () => { };

    public static Func<bool> True => () => true;
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public static class Stub<T>
{
    public static Action<T> Nop => _ => { };

    public static Func<T, bool> True => _ => true;
}