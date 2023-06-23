using MediatR;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using System.Security.Claims;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.CreatePost;

public sealed record CreatePostCommand(CreatePostDetails Details, ClaimsPrincipal CurrentUser) : IRequest<Result<Guid>>;