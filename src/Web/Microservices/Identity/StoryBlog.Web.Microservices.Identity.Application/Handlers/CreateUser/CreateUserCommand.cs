using MediatR;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Application.Handlers.CreateUser;

public sealed record CreateUserCommand(
    string Email,
    string Password
) : IRequest<Result<StoryBlogUser>>;