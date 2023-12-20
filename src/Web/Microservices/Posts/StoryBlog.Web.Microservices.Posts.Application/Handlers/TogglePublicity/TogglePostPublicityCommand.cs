using MediatR;
using StoryBlog.Web.Common.Result;
using System.Security.Claims;
using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.TogglePublicity;

public sealed record TogglePostPublicityCommand(
    string SlugOrKey,
    VisibilityStatus VisibilityStatus,
    ClaimsPrincipal CurrentUser
) : IRequest<Result>;