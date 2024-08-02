using MediatR;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Shared.Models;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.Test;

public sealed class TestCommand(Guid postKey) : IRequest<Result<PostModel>>
{
    public Guid PostKey { get; } = postKey;
}