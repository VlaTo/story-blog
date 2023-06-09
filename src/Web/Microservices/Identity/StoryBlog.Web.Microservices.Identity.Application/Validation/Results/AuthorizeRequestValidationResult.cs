﻿using StoryBlog.Web.Microservices.Identity.Application.Validation.Requests;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

/// <summary>
/// Validation result for authorize requests
/// </summary>
public class AuthorizeRequestValidationResult : ValidationResult
{
    /// <summary>
    /// Gets or sets the validated request.
    /// </summary>
    /// <value>
    /// The validated request.
    /// </value>
    public ValidatedAuthorizeRequest ValidatedRequest
    {
        get;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeRequestValidationResult"/> class.
    /// </summary>
    /// <param name="request">The request.</param>
    public AuthorizeRequestValidationResult(ValidatedAuthorizeRequest request)
        : base(false)
    {
        ValidatedRequest = request;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeRequestValidationResult" /> class.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="error">The error.</param>
    /// <param name="errorDescription">The error description.</param>
    public AuthorizeRequestValidationResult(
        ValidatedAuthorizeRequest request,
        string error,
        string? errorDescription = null)
        : base(true, error, errorDescription)
    {
        ValidatedRequest = request;
    }
}