using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class StoryBlogUserRoleClaim : IdentityRoleClaim<string>, IEntity
{
    public string? Description
    {
        get;
        set;
    }

    public string? Group
    {
        get;
        set;
    }

    public DateTimeOffset Created
    {
        get;
        set;
    }

    public DateTimeOffset? Modified
    {
        get;
        set;
    }
}