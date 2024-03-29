﻿using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

/// <summary>
/// Models storage for keys.
/// </summary>
public class Key : IEntity, IHasId<string>
{
    public string Id
    {
        get;
        set;
    }

    public int Version
    {
        get;
        set;
    }

    public DateTimeOffset Created
    {
        get;
        set;
    }

    public string Use
    {
        get;
        set;
    }

    public string Algorithm
    {
        get;
        set;
    }

    public bool IsX509Certificate
    {
        get;
        set;
    }

    public bool DataProtected
    {
        get;
        set;
    }

    public string Data
    {
        get;
        set;
    }
}