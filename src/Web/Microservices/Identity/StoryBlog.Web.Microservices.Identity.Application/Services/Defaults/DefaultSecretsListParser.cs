using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Models;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Application.Services.Defaults;

/// <summary>
/// Uses the registered secret parsers to parse a secret on the current request
/// </summary>
internal sealed class DefaultSecretsListParser : ISecretsListParser
{
    private readonly ILogger logger;
    private readonly IEnumerable<ISecretParser> parsers;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecretParser"/> class.
    /// </summary>
    /// <param name="parsers">The parsers.</param>
    /// <param name="logger">The logger.</param>
    public DefaultSecretsListParser(IEnumerable<ISecretParser> parsers, ILogger<ISecretsListParser> logger)
    {
        this.parsers = parsers;
        this.logger = logger;
    }

    /// <summary>
    /// Checks the context to find a secret.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns></returns>
    public async Task<ParsedSecret?> ParseAsync(HttpContext context)
    {
        // see if a registered parser finds a secret on the request
        ParsedSecret? bestSecret = null;

        foreach (var parser in parsers)
        {
            var parsedSecret = await parser.ParseAsync(context);

            if (null != parsedSecret)
            {
                logger.LogDebug("Parser found secret: {type}", parser.GetType().Name);

                bestSecret = parsedSecret;

                if (IdentityServerConstants.ParsedSecretTypes.NoSecret != parsedSecret.Type)
                {
                    break;
                }
            }
        }

        if (null != bestSecret)
        {
            logger.LogDebug("Secret id found: {id}", bestSecret.Id);
            return bestSecret;
        }

        logger.LogDebug("Parser found no secret");

        return null;
    }

    /// <summary>
    /// Gets all available authentication methods.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetAvailableAuthenticationMethods()
    {
        return parsers
            .Select(p => p.AuthenticationMethod)
            .Where(p => !String.IsNullOrWhiteSpace(p));
    }
}