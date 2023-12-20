using System.Security.Claims;
using MediatR;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.DeletePost;

public sealed record DeletePostCommand(
    string SlugOrKey,
    ClaimsPrincipal CurrentUser
) : IRequest<Result<Success, NotFound>>;