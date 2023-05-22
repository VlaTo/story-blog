﻿namespace StoryBlog.Web.Microservices.Identity.Application.Configuration;

/// <summary>
/// Content Security Policy Level
/// </summary>
public enum CspLevel
{
    /// <summary>
    /// Level 1
    /// </summary>
    One = 0,

    /// <summary>
    /// Level 2
    /// </summary>
    Two = 1
}

/// <summary>
/// Options for Content Security Policy
/// </summary>
public class CspOptions
{
    /// <summary>
    /// Gets or sets the minimum CSP level.
    /// </summary>
    public CspLevel Level
    {
        get;
        set;
    }

    /// <summary>
    /// Gets or sets a value indicating whether the deprecated X-Content-Security-Policy header should be added.
    /// </summary>
    public bool AddDeprecatedHeader
    {
        get;
        set;
    }

    public CspOptions()
    {
        Level = CspLevel.Two;
        AddDeprecatedHeader = true;
    }
}