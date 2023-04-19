using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Core.Extensions;

internal static class ClassBuilderExtensions
{
    public static IClassBuilder<TComponent> Name<TComponent>(this IClassBuilder<TComponent> classBuilder, string name)
        where TComponent : ComponentBase
    {
        return classBuilder.Name(_ => name);
    }
}