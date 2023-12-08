using System.Security.Principal;
using MediatR;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.DeletePost;

public sealed record DeletePostCommand(
    string SlugOrKey,
    IPrincipal CurrentUser
) : IRequest<Result<Success, NotFound>>;