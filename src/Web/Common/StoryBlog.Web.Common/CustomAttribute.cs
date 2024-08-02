using System.Reflection;

namespace StoryBlog.Web.Common;

public static class CustomAttributes
{
    public static TAttribute? GetCustomAttribute<TAttribute>(this Assembly? assembly)
        where TAttribute : Attribute
    {
        return null != assembly ? (TAttribute?)Attribute.GetCustomAttribute(assembly, typeof(TAttribute)) : default;
    }
}