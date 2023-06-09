﻿namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

/// <summary>
/// Token usage types.
/// </summary>
public enum TokenUsage
{
    /// <summary>
    /// Re-use the refresh token handle
    /// </summary>
    ReUse = 0,

    /// <summary>
    /// Issue a new refresh token handle every time
    /// </summary>
    OneTimeOnly = 1
}