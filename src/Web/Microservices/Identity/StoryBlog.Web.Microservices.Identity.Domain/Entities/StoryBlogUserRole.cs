﻿using Microsoft.AspNetCore.Identity;
using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public sealed class StoryBlogUserRole : IdentityRole, IEntity, IHasId<string>
{
    public string? Description
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
}