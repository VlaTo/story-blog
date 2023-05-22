using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Application.Contexts;

public interface IStoryBlogIdentityDbContext
{
    /*DbSet<StoryBlogUser> Users
    {
        get;
    }

    DbSet<StoryBlogUserRole> Roles
    {
        get;
    }

    DbSet<IdentityRoleClaim<string>> RoleClaims
    {
        get;
    }

    DbSet<IdentityUserClaim<string>> UserClaims
    {
        get;
    }

    DbSet<IdentityUserRole<string>> UserRoles
    {
        get;
    }

    DbSet<IdentityUserLogin<string>> UserLogins
    {
        get;
    }

    DbSet<IdentityUserToken<string>> UserTokens
    {
        get;
    }*/
}