using System.Security.Claims;
using MediatR;
using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;

public sealed record GetPostModelQuery(Guid PostKey, ClaimsPrincipal? Principal) : IRequest<Result<Models.Post>>;