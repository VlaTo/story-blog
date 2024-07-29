using MediatR;
using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.Test;

public sealed class TestCommand(Guid postKey) : IRequest<Result>
{
    public Guid PostKey { get; } = postKey;
}