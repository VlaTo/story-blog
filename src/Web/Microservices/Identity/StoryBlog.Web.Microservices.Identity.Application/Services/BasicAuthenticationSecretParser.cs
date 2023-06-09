﻿using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.Models;
using System.Text;

namespace StoryBlog.Web.Microservices.Identity.Application.Services;

/// <summary>
/// Parses a Basic Authentication header
/// </summary>
public sealed class BasicAuthenticationSecretParser : ISecretParser
{
    private readonly ILogger logger;
    private readonly IdentityServerOptions options;

    /// <summary>
    /// Returns the authentication method name that this parser implements
    /// </summary>
    /// <value>
    /// The authentication method.
    /// </value>
    public string AuthenticationMethod => OidcConstants.EndpointAuthenticationMethods.BasicAuthentication;

    /// <summary>
    /// Creates the parser with a reference to identity server options
    /// </summary>
    /// <param name="options">IdentityServer options</param>
    /// <param name="logger">The logger</param>
    public BasicAuthenticationSecretParser(
        IdentityServerOptions options,
        ILogger<BasicAuthenticationSecretParser> logger)
    {
        this.options = options;
        this.logger = logger;
    }

    /// <summary>
    /// Tries to find a secret that can be used for authentication
    /// </summary>
    /// <returns>
    /// A parsed secret
    /// </returns>
    public Task<ParsedSecret?> ParseAsync(HttpContext context)
    {
        const string basicHeader = "Basic ";

        logger.LogDebug("Start parsing Basic Authentication secret");

        var notFound = Task.FromResult<ParsedSecret?>(null);
        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (authorizationHeader.IsMissing())
        {
            return notFound;
        }

        if (false == authorizationHeader.StartsWith(basicHeader, StringComparison.OrdinalIgnoreCase))
        {
            return notFound;
        }

        var parameter = authorizationHeader.Substring(basicHeader.Length);
        string pair;

        try
        {
            pair = Encoding.UTF8.GetString(Convert.FromBase64String(parameter));
        }
        catch (FormatException)
        {
            logger.LogWarning("Malformed Basic Authentication credential.");
            return notFound;
        }
        catch (ArgumentException)
        {
            logger.LogWarning("Malformed Basic Authentication credential.");
            return notFound;
        }

        var ix = pair.IndexOf(':');

        if (-1 == ix)
        {
            logger.LogWarning("Malformed Basic Authentication credential.");
            return notFound;
        }

        var clientId = pair.Substring(0, ix);
        var secret = pair.Substring(ix + 1);

        if (clientId.IsPresent())
        {
            if (options.InputLengthRestrictions.ClientId < clientId.Length)
            {
                logger.LogError("Client ID exceeds maximum length.");
                return notFound;
            }

            if (secret.IsPresent())
            {
                if (options.InputLengthRestrictions.ClientSecret < secret.Length)
                {
                    logger.LogError("Client secret exceeds maximum length.");
                    return notFound;
                }

                var parsedSecret = new ParsedSecret
                {
                    Id = Decode(clientId),
                    Credential = Decode(secret),
                    Type = IdentityServerConstants.ParsedSecretTypes.SharedSecret
                };

                return Task.FromResult<ParsedSecret?>(parsedSecret);
            }
            else
            {
                // client secret is optional
                logger.LogDebug("client id without secret found");

                var parsedSecret = new ParsedSecret
                {
                    Id = Decode(clientId),
                    Type = IdentityServerConstants.ParsedSecretTypes.NoSecret
                };

                return Task.FromResult<ParsedSecret?>(parsedSecret);
            }
        }

        logger.LogDebug("No Basic Authentication secret found");

        return notFound;
    }

    // RFC6749 says individual values must be application/x-www-form-urlencoded
    // 2.3.1
    private static string Decode(string value)
    {
        return value.IsMissing() ? string.Empty : Uri.UnescapeDataString(value.Replace("+", "%20"));
    }
}