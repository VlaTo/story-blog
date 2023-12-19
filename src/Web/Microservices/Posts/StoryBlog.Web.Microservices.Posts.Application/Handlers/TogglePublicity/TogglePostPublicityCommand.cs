using MediatR;
using StoryBlog.Web.Common.Result;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.TogglePublicity;

public sealed record TogglePostPublicityCommand(
    string SlugOrKey,
    bool IsPublic,
    ClaimsPrincipal CurrentUser
) : IRequest<Result<(Guid PostKey, bool IsPublic)>>;