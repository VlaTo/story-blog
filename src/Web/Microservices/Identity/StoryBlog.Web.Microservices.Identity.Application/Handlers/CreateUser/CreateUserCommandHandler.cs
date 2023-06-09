using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Identity.Core;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Application.Handlers.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Common.Application.Result<StoryBlogUser>>
{
    private readonly UserManager<StoryBlogUser> userManager;
    private readonly SignInManager<StoryBlogUser> signInManager;
    private readonly RoleManager<StoryBlogUserRole> roleManager;
    private readonly ISystemClock clock;
    private readonly IOptions<StoryBlogIdentityServerOptions<StoryBlogIdentityOptions>> options;

    public CreateUserCommandHandler(
        UserManager<StoryBlogUser> userManager,
        SignInManager<StoryBlogUser> signInManager,
        RoleManager<StoryBlogUserRole> roleManager,
        ISystemClock clock,
        IOptions<StoryBlogIdentityServerOptions<StoryBlogIdentityOptions>> options)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
        this.clock = clock;
        this.options = options;
    }

    public async Task<Common.Application.Result<StoryBlogUser>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new StoryBlogUser
        {
            UserName = request.Email,
            Email = request.Email,
            IsActive = true
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {

            result = await userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, "Guest", ClaimValueTypes.String));

            var viewerRole = new StoryBlogUserRole
            {
                Name = Permissions.Blogs.View,
                Description = "Blog Viewer"
            };

            result = await roleManager.CreateAsync(viewerRole);

            if (false == result.Succeeded)
            {
                return new Common.Application.Result<StoryBlogUser>(new Exception());
            }

            result = await roleManager.AddClaimAsync(viewerRole, new Claim(ClaimTypes.Actor, "Viewer", ClaimValueTypes.String));
            result = await userManager.AddToRoleAsync(user, viewerRole.Name);

            return new Common.Application.Result<StoryBlogUser>(user);
        }

        return new Common.Application.Result<StoryBlogUser>(new Exception());
    }
}