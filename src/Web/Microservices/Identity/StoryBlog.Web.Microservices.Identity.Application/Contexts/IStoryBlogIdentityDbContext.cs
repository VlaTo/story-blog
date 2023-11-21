using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Application.Contexts;

public interface IStoryBlogIdentityDbContext
{
    DbSet<StoryBlogUser> Users
    {
        get;
    }

    DbSet<StoryBlogRole> Roles
    {
        get;
    }

    DbSet<StoryBlogRoleClaim> RoleClaims
    {
        get;
    }

    DbSet<StoryBlogUserClaim> UserClaims
    {
        get;
    }

    DbSet<StoryBlogUserRole> UserRoles
    {
        get;
    }

    DbSet<StoryBlogUserLogin> UserLogins
    {
        get;
    }

    DbSet<StoryBlogUserToken> UserTokens
    {
        get;
    }
}