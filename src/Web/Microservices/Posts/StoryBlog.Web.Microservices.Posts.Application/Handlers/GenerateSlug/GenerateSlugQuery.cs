using MediatR;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GenerateSlug;

public sealed class GenerateSlugQuery : IRequest<string?>
{
    public string Title
    {
        get;
    }

    public GenerateSlugQuery(string title)
    {
        Title = title;
    }
}