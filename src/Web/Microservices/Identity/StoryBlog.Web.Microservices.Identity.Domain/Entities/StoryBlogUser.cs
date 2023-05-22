using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class StoryBlogUser : IdentityUser, IEntity, IHasId<string>
{
    public bool IsActive
    {
        get;
        set;
    }
    
    public DateTime Created
    {
        get;
        set;
    }

    public DateTime? Modified
    {
        get;
        set;
    }

    public string? RefreshToken
    {
        get;
        set;
    }
    
    public DateTime? RefreshTokenExpiryTime
    {
        get;
        set;
    }
}