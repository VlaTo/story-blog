using System.Security.Claims;
using MediatR;
using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;

public sealed record GetPostQuery(Guid Key, ClaimsPrincipal CurrentUser) : IRequest<Result<Models.Post>>;