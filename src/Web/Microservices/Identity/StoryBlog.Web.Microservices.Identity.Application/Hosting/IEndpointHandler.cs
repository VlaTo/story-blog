﻿using Microsoft.AspNetCore.Http;

namespace StoryBlog.Web.Microservices.Identity.Application.Hosting;

/// <summary>
/// Endpoint handler
/// </summary>
public interface IEndpointHandler
{
    /// <summary>
    /// Processes the request.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns></returns>
    Task<IEndpointResult?> ProcessAsync(HttpContext context);
}