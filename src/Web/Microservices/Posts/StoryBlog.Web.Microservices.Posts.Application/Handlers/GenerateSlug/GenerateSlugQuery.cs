using MediatR;
using StoryBlog.Web.Common.Application;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GenerateSlug;

public sealed class GenerateSlugQuery : IRequest<Result<string>>
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