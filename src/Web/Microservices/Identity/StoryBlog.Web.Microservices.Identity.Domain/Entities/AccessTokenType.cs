﻿namespace StoryBlog.Web.Microservices.Identity.Domain.Entities;

/// <summary>
/// Access token types.
/// </summary>
public enum AccessTokenType
{
    /// <summary>
    /// Self-contained Json Web Token
    /// </summary>
    Jwt = 0,

    /// <summary>
    /// Reference token
    /// </summary>
    Reference = 1
}