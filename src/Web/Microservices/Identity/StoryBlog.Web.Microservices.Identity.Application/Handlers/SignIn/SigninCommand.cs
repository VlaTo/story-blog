using MediatR;
using StoryBlog.Infrastructure.AnyOf;
using StoryBlog.Web.Microservices.Identity.Application.Models.Requests;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Application.Handlers.SigningIn;

public sealed record SigninCommand(
    AuthorizationRequest? Context,
    string Email,
    string Password,
    bool RememberMe
) : IRequest<AnyOf<SignInError, (StoryBlogUser User, string? Token)>>;