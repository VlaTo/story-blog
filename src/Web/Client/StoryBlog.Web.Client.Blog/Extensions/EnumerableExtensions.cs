namespace StoryBlog.Web.Client.Blog.Extensions;

internal static class EnumerableExtensions
{
    public static IEnumerable<T> Exclude<T>(this IEnumerable<T> enumerable, Predicate<T> selector)
    {
        foreach (var obj in enumerable)
        {
            if (selector.Invoke(obj))
            {
                continue;
            }

            yield return obj;
        }
    }

    public static IEnumerable<T> MapReduce<T>(this IEnumerable<T> enumerable, Predicate<T> selector, Func<T, T> map)
    {
        foreach (var obj in enumerable)
        {
            var temp = obj;

            if (selector.Invoke(obj))
            {
                temp = map.Invoke(obj);
            }

            yield return temp;
        }
    }
}