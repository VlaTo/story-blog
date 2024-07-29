﻿using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

public class ApiResourceScope : IEntity, IHasId<int>
{
    public int Id
    {
        get;
        set;
    }

    public required string Scope
    {
        get;
        set;
    }

    public int ApiResourceId
    {
        get;
        set;
    }

    public required ApiResource ApiResource
    {
        get;
        set;
    }
}