﻿using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;

namespace StoryBlog.Web.Microservices.Identity.Application.Configuration;

/// <summary>
/// Options for CORS
/// </summary>
public class CorsOptions
{
    /// <summary>
    /// Gets or sets the name of the cors policy.
    /// </summary>
    /// <value>
    /// The name of the cors policy.
    /// </value>
    public string CorsPolicyName
    {
        get;
        set;
    }

    /// <summary>
    /// The value to be used in the preflight `Access-Control-Max-Age` response header.
    /// </summary>
    public TimeSpan? PreflightCacheDuration
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets the cors paths.
    /// </summary>
    /// <value>
    /// The cors paths.
    /// </value>
    public ICollection<PathString> CorsPaths
    {
        get;
        set;
    }

    public CorsOptions()
    {
        CorsPolicyName = IdentityServerConstants.IdentityServerName;
        CorsPaths = Constants.ProtocolRoutePaths.CorsPaths
            .Select(x => new PathString(x.EnsureLeadingSlash()))
            .ToList();
    }
}