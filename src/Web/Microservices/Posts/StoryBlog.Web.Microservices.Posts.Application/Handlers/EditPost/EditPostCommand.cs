using MediatR;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.EditPost;

public sealed record EditPostCommand(Guid Key, EditPostDetails Details, ClaimsPrincipal CurrentUser) : IRequest<Result>;