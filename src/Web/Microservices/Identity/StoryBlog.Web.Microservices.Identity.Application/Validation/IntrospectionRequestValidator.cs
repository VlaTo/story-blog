using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;
using System.Collections.Specialized;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

/// <summary>
/// The introspection request validator
/// </summary>
/// <seealso cref="IIntrospectionRequestValidator" />
internal sealed class IntrospectionRequestValidator : IIntrospectionRequestValidator
{
    private readonly ILogger logger;
    private readonly ITokenValidator tokenValidator;

    /// <summary>
    /// Initializes a new instance of the <see cref="IntrospectionRequestValidator"/> class.
    /// </summary>
    /// <param name="tokenValidator">The token validator.</param>
    /// <param name="logger">The logger.</param>
    public IntrospectionRequestValidator(
        ITokenValidator tokenValidator,
        ILogger<IntrospectionRequestValidator> logger)
    {
        this.tokenValidator = tokenValidator;
        this.logger = logger;
    }

    public async Task<IntrospectionRequestValidationResult> ValidateAsync(NameValueCollection parameters, ApiResource api)
    {
        using var activity = Tracing.BasicActivitySource.StartActivity("IntrospectionRequestValidator.Validate");

        logger.LogDebug("Introspection request validation started.");

        // retrieve required token
        var token = parameters.Get("token");

        if (token == null)
        {
            logger.LogError("Token is missing");

            return new IntrospectionRequestValidationResult
            {
                IsError = true,
                Api = api,
                Error = "missing_token",
                Parameters = parameters
            };
        }

        // validate token
        var tokenValidationResult = await tokenValidator.ValidateAccessTokenAsync(token);

        // invalid or unknown token
        if (tokenValidationResult.IsError)
        {
            logger.LogDebug("Token is invalid.");

            return new IntrospectionRequestValidationResult
            {
                IsActive = false,
                IsError = false,
                Token = token,
                Api = api,
                Parameters = parameters
            };
        }

        logger.LogDebug("Introspection request validation successful.");

        // valid token
        return new IntrospectionRequestValidationResult
        {
            IsActive = true,
            IsError = false,
            Token = token,
            Claims = tokenValidationResult.Claims,
            Api = api,
            Parameters = parameters
        };
    }
}