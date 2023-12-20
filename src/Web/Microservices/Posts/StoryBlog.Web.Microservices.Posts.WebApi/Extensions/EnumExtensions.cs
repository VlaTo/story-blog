namespace StoryBlog.Web.Microservices.Posts.WebApi.Extensions;

internal static class EnumExtensions
{
    public static TDestination AsEnum<TDestination>(this Enum source)
        where TDestination : Enum
    {
        var sourceName = Enum.GetName(source.GetType(), source);

        if (Enum.TryParse(typeof(TDestination), sourceName, out var result))
        {
            return (TDestination)result;
        }

        throw new ArgumentException("Invalid convert", nameof(source));
    }
}