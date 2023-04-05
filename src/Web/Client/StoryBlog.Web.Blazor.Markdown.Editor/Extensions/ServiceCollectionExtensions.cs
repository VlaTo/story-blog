using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Blazor.Markdown.Editor.Configuration;

namespace StoryBlog.Web.Blazor.Markdown.Editor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMarkdownEditor(this IServiceCollection services, Action<MarkdownEditorOptions> configuration)
    {

        return services;
    }
}