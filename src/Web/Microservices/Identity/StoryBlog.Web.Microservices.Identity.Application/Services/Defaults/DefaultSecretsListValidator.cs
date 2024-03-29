﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Models;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Application.Validation.Results;

namespace StoryBlog.Web.Microservices.Identity.Application.Services.Defaults;

/// <summary>
/// Validates secrets using the registered validators
/// </summary>
internal sealed class DefaultSecretsListValidator : ISecretsListValidator
{
    private readonly ISystemClock clock;
    private readonly IEnumerable<ISecretValidator> validators;
    private readonly ILogger logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecretValidator"/> class.
    /// </summary>
    /// <param name="clock">The clock.</param>
    /// <param name="validators">The validators.</param>
    /// <param name="logger">The logger.</param>
    public DefaultSecretsListValidator(
        ISystemClock clock,
        IEnumerable<ISecretValidator> validators,
        ILogger<ISecretsListValidator> logger)
    {
        this.clock = clock;
        this.validators = validators;
        this.logger = logger;
    }

    /// <summary>
    /// Validates the secret.
    /// </summary>
    /// <param name="parsedSecret">The parsed secret.</param>
    /// <param name="secrets">The secrets.</param>
    /// <returns></returns>
    public async Task<SecretValidationResult> ValidateAsync(IEnumerable<Secret> secrets, ParsedSecret parsedSecret)
    {
        var secretsArray = secrets as Secret[] ?? secrets.ToArray();
        var expiredSecrets = secretsArray
            .Where(s => s.Expiration.HasExpired(clock.UtcNow))
            .ToList();

        if (expiredSecrets.Any())
        {
            expiredSecrets.ForEach(
                ex => logger.LogInformation("Secret [{description}] is expired", ex.Description ?? "no description")
            );
        }

        var currentSecrets = secretsArray
            .Where(s => false == s.Expiration.HasExpired(clock.UtcNow))
            .ToArray();

        // see if a registered validator can validate the secret
        foreach (var validator in validators)
        {
            var secretValidationResult = await validator.ValidateAsync(currentSecrets, parsedSecret);

            if (secretValidationResult.Success)
            {
                logger.LogDebug("Secret validator success: {0}", validator.GetType().Name);

                return secretValidationResult;
            }
        }

        logger.LogDebug("Secret validators could not validate secret");

        return new SecretValidationResult
        {
            Success = false
        };
    }
}