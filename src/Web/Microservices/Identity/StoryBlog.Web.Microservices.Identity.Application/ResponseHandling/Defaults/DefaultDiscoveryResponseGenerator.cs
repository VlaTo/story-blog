﻿using IdentityModel;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.Extensions;
using StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Generators;
using StoryBlog.Web.Microservices.Identity.Application.Services;
using StoryBlog.Web.Microservices.Identity.Application.Stores;
using StoryBlog.Web.Microservices.Identity.Application.Validation;
using System.Security.Cryptography;
using StoryBlog.Web.Common.Identity.Permission;
using OidcConstants = IdentityModel.OidcConstants;

namespace StoryBlog.Web.Microservices.Identity.Application.ResponseHandling.Defaults;

/// <summary>
/// Default implementation of the discovery endpoint response generator
/// </summary>
/// <seealso cref="IDiscoveryResponseGenerator" />
public class DefaultDiscoveryResponseGenerator : IDiscoveryResponseGenerator
{
    /// <summary>
    /// The options
    /// </summary>
    protected IdentityServerOptions Options
    {
        get;
        init;
    }

    /// <summary>
    /// The key material service
    /// </summary>
    protected IKeyMaterialService Keys
    {
        get;
        init;
    }

    /// <summary>
    /// The resource store
    /// </summary>
    protected IResourceStore ResourceStore
    {
        get;
        init;
    }

    /// <summary>
    /// The secret parsers
    /// </summary>
    protected ISecretsListParser SecretParsers
    {
        get;
        init;
    }

    /// <summary>
    /// The resource owner validator
    /// </summary>
    protected IResourceOwnerPasswordValidator ResourceOwnerValidator
    {
        get;
        init;
    }

    /// <summary>
    /// The extension grants validator
    /// </summary>
    protected ExtensionGrantValidator ExtensionGrants
    {
        get;
        init;
    }

    /// <summary>
    /// The logger
    /// </summary>
    protected ILogger Logger
    {
        get;
        init;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultDiscoveryResponseGenerator"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="resourceStore">The resource store.</param>
    /// <param name="keys">The keys.</param>
    /// <param name="extensionGrants">The extension grants.</param>
    /// <param name="secretParsers">The secret parsers.</param>
    /// <param name="resourceOwnerValidator">The resource owner validator.</param>
    /// <param name="logger">The logger.</param>
    public DefaultDiscoveryResponseGenerator(
        IdentityServerOptions options,
        IResourceStore resourceStore,
        IKeyMaterialService keys,
        ExtensionGrantValidator extensionGrants,
        ISecretsListParser secretParsers,
        IResourceOwnerPasswordValidator resourceOwnerValidator,
        ILogger<DefaultDiscoveryResponseGenerator> logger)
    {
        Options = options;
        ResourceStore = resourceStore;
        Keys = keys;
        ExtensionGrants = extensionGrants;
        SecretParsers = secretParsers;
        ResourceOwnerValidator = resourceOwnerValidator;
        Logger = logger;
    }

    /// <summary>
    /// Creates the discovery document.
    /// </summary>
    /// <param name="baseUrl">The base URL.</param>
    /// <param name="issuerUri">The issuer URI.</param>
    public virtual async Task<Dictionary<string, object>> CreateDiscoveryDocumentAsync(string? baseUrl, string issuerUri)
    {
        using var activity = Tracing.ActivitySource.StartActivity("DiscoveryResponseGenerator.CreateDiscoveryDocument");

        baseUrl = baseUrl.EnsureTrailingSlash();

        var entries = new Dictionary<string, object>
        {
            { OidcConstants.Discovery.Issuer, issuerUri }
        };

        // jwks
        if (Options.Discovery.ShowKeySet)
        {
            if ((await Keys.GetValidationKeysAsync()).Any())
            {
                entries.Add(OidcConstants.Discovery.JwksUri, baseUrl + Constants.ProtocolRoutePaths.DiscoveryWebKeys);
            }
        }

        // endpoints
        if (Options.Discovery.ShowEndpoints)
        {
            if (Options.Endpoints.EnableAuthorizeEndpoint)
            {
                entries.Add(OidcConstants.Discovery.AuthorizationEndpoint, baseUrl + Constants.ProtocolRoutePaths.Authorize);
            }

            if (Options.Endpoints.EnableTokenEndpoint)
            {
                entries.Add(OidcConstants.Discovery.TokenEndpoint, baseUrl + Constants.ProtocolRoutePaths.Token);
            }

            if (Options.Endpoints.EnableUserInfoEndpoint)
            {
                entries.Add(OidcConstants.Discovery.UserInfoEndpoint, baseUrl + Constants.ProtocolRoutePaths.UserInfo);
            }

            if (Options.Endpoints.EnableEndSessionEndpoint)
            {
                entries.Add(OidcConstants.Discovery.EndSessionEndpoint, baseUrl + Constants.ProtocolRoutePaths.EndSession);
            }

            if (Options.Endpoints.EnableCheckSessionEndpoint)
            {
                entries.Add(OidcConstants.Discovery.CheckSessionIframe, baseUrl + Constants.ProtocolRoutePaths.CheckSession);
            }

            if (Options.Endpoints.EnableTokenRevocationEndpoint)
            {
                entries.Add(OidcConstants.Discovery.RevocationEndpoint, baseUrl + Constants.ProtocolRoutePaths.Revocation);
            }

            if (Options.Endpoints.EnableIntrospectionEndpoint)
            {
                entries.Add(OidcConstants.Discovery.IntrospectionEndpoint, baseUrl + Constants.ProtocolRoutePaths.Introspection);
            }

            if (Options.Endpoints.EnableDeviceAuthorizationEndpoint)
            {
                entries.Add(OidcConstants.Discovery.DeviceAuthorizationEndpoint, baseUrl + Constants.ProtocolRoutePaths.DeviceAuthorization);
            }

            if (Options.Endpoints.EnableBackchannelAuthenticationEndpoint)
            {
                entries.Add(OidcConstants.Discovery.BackchannelAuthenticationEndpoint, baseUrl + Constants.ProtocolRoutePaths.BackchannelAuthentication);
            }

            if (Options.MutualTls.Enabled)
            {
                var mtlsEndpoints = new Dictionary<string, string>();

                if (Options.Endpoints.EnableTokenEndpoint)
                {
                    mtlsEndpoints.Add(OidcConstants.Discovery.TokenEndpoint, ConstructMtlsEndpoint(Constants.ProtocolRoutePaths.Token));
                }
                if (Options.Endpoints.EnableTokenRevocationEndpoint)
                {
                    mtlsEndpoints.Add(OidcConstants.Discovery.RevocationEndpoint, ConstructMtlsEndpoint(Constants.ProtocolRoutePaths.Revocation));
                }
                if (Options.Endpoints.EnableIntrospectionEndpoint)
                {
                    mtlsEndpoints.Add(OidcConstants.Discovery.IntrospectionEndpoint, ConstructMtlsEndpoint(Constants.ProtocolRoutePaths.Introspection));
                }
                if (Options.Endpoints.EnableDeviceAuthorizationEndpoint)
                {
                    mtlsEndpoints.Add(OidcConstants.Discovery.DeviceAuthorizationEndpoint, ConstructMtlsEndpoint(Constants.ProtocolRoutePaths.DeviceAuthorization));
                }

                if (mtlsEndpoints.Any())
                {
                    entries.Add(OidcConstants.Discovery.MtlsEndpointAliases, mtlsEndpoints);
                }

                string ConstructMtlsEndpoint(string endpoint)
                {
                    // path based
                    if (Options.MutualTls.DomainName.IsMissing())
                    {
                        return baseUrl + endpoint.Replace(Constants.ProtocolRoutePaths.ConnectPathPrefix, Constants.ProtocolRoutePaths.MtlsPathPrefix);
                    }

                    // domain based
                    if (Options.MutualTls.DomainName.Contains("."))
                    {
                        return $"https://{Options.MutualTls.DomainName}/{endpoint}";
                    }
                    // sub-domain based
                    else
                    {
                        var parts = baseUrl.Split("://");
                        return $"https://{Options.MutualTls.DomainName}.{parts[1]}{endpoint}";
                    }
                }
            }
        }

        // logout
        if (Options.Endpoints.EnableEndSessionEndpoint)
        {
            entries.Add(OidcConstants.Discovery.FrontChannelLogoutSupported, true);
            entries.Add(OidcConstants.Discovery.FrontChannelLogoutSessionSupported, true);
            entries.Add(OidcConstants.Discovery.BackChannelLogoutSupported, true);
            entries.Add(OidcConstants.Discovery.BackChannelLogoutSessionSupported, true);
        }

        // scopes and claims
        if (Options.Discovery.ShowIdentityScopes ||
            Options.Discovery.ShowApiScopes ||
            Options.Discovery.ShowClaims)
        {
            var resources = await ResourceStore.GetAllEnabledResourcesAsync();
            var scopes = new List<string>();

            // scopes
            if (Options.Discovery.ShowIdentityScopes)
            {
                scopes.AddRange(resources.IdentityResources.Where(x => x.ShowInDiscoveryDocument).Select(x => x.Name));
            }

            if (Options.Discovery.ShowApiScopes)
            {
                var apiScopes = from scope in resources.ApiScopes
                                where scope.ShowInDiscoveryDocument
                                select scope.Name;

                scopes.AddRange(apiScopes);
                scopes.Add(OidcConstants.StandardScopes.OfflineAccess);
            }

            if (scopes.Any())
            {
                entries.Add(OidcConstants.Discovery.ScopesSupported, scopes.ToArray());
            }

            // claims
            if (Options.Discovery.ShowClaims)
            {
                var claims = new List<string>();

                // add non-hidden identity scopes related claims
                claims.AddRange(resources.IdentityResources.Where(x => x.ShowInDiscoveryDocument).SelectMany(x => x.UserClaims));
                claims.AddRange(resources.ApiResources.Where(x => x.ShowInDiscoveryDocument).SelectMany(x => x.UserClaims));
                claims.AddRange(resources.ApiScopes.Where(x => x.ShowInDiscoveryDocument).SelectMany(x => x.UserClaims));

                entries.Add(OidcConstants.Discovery.ClaimsSupported, claims.Distinct().ToArray());
            }
        }

        // grant types
        if (Options.Discovery.ShowGrantTypes)
        {
            var standardGrantTypes = new List<string>
            {
                OidcConstants.GrantTypes.AuthorizationCode,
                OidcConstants.GrantTypes.ClientCredentials,
                OidcConstants.GrantTypes.RefreshToken,
                OidcConstants.GrantTypes.Implicit
            };

            if (false == (ResourceOwnerValidator is NotSupportedResourceOwnerPasswordValidator))
            {
                standardGrantTypes.Add(OidcConstants.GrantTypes.Password);
            }

            if (Options.Endpoints.EnableDeviceAuthorizationEndpoint)
            {
                standardGrantTypes.Add(OidcConstants.GrantTypes.DeviceCode);
            }

            if (Options.Endpoints.EnableBackchannelAuthenticationEndpoint)
            {
                standardGrantTypes.Add(OidcConstants.GrantTypes.Ciba);
            }

            var showGrantTypes = new List<string>(standardGrantTypes);

            if (Options.Discovery.ShowExtensionGrantTypes)
            {
                showGrantTypes.AddRange(ExtensionGrants.GetAvailableGrantTypes());
            }

            entries.Add(OidcConstants.Discovery.GrantTypesSupported, showGrantTypes.ToArray());
        }

        // response types
        if (Options.Discovery.ShowResponseTypes)
        {
            entries.Add(OidcConstants.Discovery.ResponseTypesSupported, IdentityServerConstants.SupportedResponseTypes.ToArray());
        }

        // response modes
        if (Options.Discovery.ShowResponseModes)
        {
            entries.Add(OidcConstants.Discovery.ResponseModesSupported, IdentityServerConstants.SupportedResponseModes.ToArray());
        }

        // misc
        if (Options.Discovery.ShowTokenEndpointAuthenticationMethods)
        {
            var types = SecretParsers.GetAvailableAuthenticationMethods().ToList();
            if (Options.MutualTls.Enabled)
            {
                types.Add(OidcConstants.EndpointAuthenticationMethods.TlsClientAuth);
                types.Add(OidcConstants.EndpointAuthenticationMethods.SelfSignedTlsClientAuth);
            }

            entries.Add(OidcConstants.Discovery.TokenEndpointAuthenticationMethodsSupported, types);
        }

        var signingCredentials = await Keys.GetAllSigningCredentialsAsync();
        if (signingCredentials.Any())
        {
            var signingAlgorithms = signingCredentials.Select(c => c.Algorithm).Distinct();
            entries.Add(OidcConstants.Discovery.IdTokenSigningAlgorithmsSupported, signingAlgorithms);
        }

        entries.Add(OidcConstants.Discovery.SubjectTypesSupported, new[] { "public" });
        entries.Add(OidcConstants.Discovery.CodeChallengeMethodsSupported, new[]
        {
            OidcConstants.CodeChallengeMethods.Plain,
            OidcConstants.CodeChallengeMethods.Sha256
        });

        if (Options.Endpoints.EnableAuthorizeEndpoint)
        {
            entries.Add(OidcConstants.Discovery.RequestParameterSupported, true);

            entries.Add(OidcConstants.Discovery.RequestObjectSigningAlgorithmsSupported, new[]
            {
                SecurityAlgorithms.RsaSha256,
                SecurityAlgorithms.RsaSha384,
                SecurityAlgorithms.RsaSha512,

                SecurityAlgorithms.RsaSsaPssSha256,
                SecurityAlgorithms.RsaSsaPssSha384,
                SecurityAlgorithms.RsaSsaPssSha512,

                SecurityAlgorithms.EcdsaSha256,
                SecurityAlgorithms.EcdsaSha384,
                SecurityAlgorithms.EcdsaSha512,

                SecurityAlgorithms.HmacSha256,
                SecurityAlgorithms.HmacSha384,
                SecurityAlgorithms.HmacSha512
            });

            if (Options.Endpoints.EnableJwtRequestUri)
            {
                entries.Add(OidcConstants.Discovery.RequestUriParameterSupported, true);
            }
        }

        entries.Add(OidcConstants.Discovery.AuthorizationResponseIssParameterSupported, true);

        if (Options.MutualTls.Enabled)
        {
            entries.Add(OidcConstants.Discovery.TlsClientCertificateBoundAccessTokens, true);
        }

        if (Options.Endpoints.EnableBackchannelAuthenticationEndpoint)
        {
            entries.Add(OidcConstants.Discovery.BackchannelTokenDeliveryModesSupported, new[] { OidcConstants.BackchannelTokenDeliveryModes.Poll });
            entries.Add(OidcConstants.Discovery.BackchannelUserCodeParameterSupported, true);
        }

        // custom entries
        if (false == Options.Discovery.CustomEntries.IsNullOrEmpty())
        {
            foreach (var (key, value) in Options.Discovery.CustomEntries)
            {
                if (entries.ContainsKey(key))
                {
                    Logger.LogError("Discovery custom entry {key} cannot be added, because it already exists.", key);
                }
                else
                {
                    if (value is string cvs)
                    {
                        if (cvs.StartsWith("~/") && Options.Discovery.ExpandRelativePathsInCustomEntries)
                        {
                            entries.Add(key, baseUrl + cvs.Substring(2));
                            continue;
                        }
                    }

                    entries.Add(key, value);
                }
            }
        }

        return entries;
    }

    /// <summary>
    /// Creates the JWK document.
    /// </summary>
    public virtual async Task<IEnumerable<IdentityModel.Jwk.JsonWebKey>> CreateJwkDocumentAsync()
    {
        using var activity = Tracing.ActivitySource.StartActivity("DiscoveryResponseGenerator.CreateJwkDocument");

        var webKeys = new List<IdentityModel.Jwk.JsonWebKey>();

        foreach (var key in await Keys.GetValidationKeysAsync())
        {
            if (key.Key is X509SecurityKey x509Key)
            {
                var cert64 = Convert.ToBase64String(x509Key.Certificate.RawData);
                var thumbprint = Base64Url.Encode(x509Key.Certificate.GetCertHash());

                if (x509Key.PublicKey is RSA rsa)
                {
                    var parameters = rsa.ExportParameters(false);
                    var exponent = Base64Url.Encode(parameters.Exponent);
                    var modulus = Base64Url.Encode(parameters.Modulus);

                    var rsaJsonWebKey = new IdentityModel.Jwk.JsonWebKey
                    {
                        Kty = "RSA",
                        Use = "sig",
                        Kid = x509Key.KeyId,
                        X5t = thumbprint,
                        E = exponent,
                        N = modulus,
                        X5c = new[] { cert64 },
                        Alg = key.SigningAlgorithm
                    };

                    webKeys.Add(rsaJsonWebKey);
                }
                else if (x509Key.PublicKey is ECDsa ecdsa)
                {
                    var parameters = ecdsa.ExportParameters(false);
                    var x = Base64Url.Encode(parameters.Q.X);
                    var y = Base64Url.Encode(parameters.Q.Y);

                    var ecdsaJsonWebKey = new IdentityModel.Jwk.JsonWebKey
                    {
                        Kty = "EC",
                        Use = "sig",
                        Kid = x509Key.KeyId,
                        X5t = thumbprint,
                        X = x,
                        Y = y,
                        Crv = CryptoHelper.GetCrvValueFromCurve(parameters.Curve),
                        X5c = new[] { cert64 },
                        Alg = key.SigningAlgorithm
                    };

                    webKeys.Add(ecdsaJsonWebKey);
                }
                else
                {
                    throw new InvalidOperationException($"key type: {x509Key.PublicKey.GetType().Name} not supported.");
                }
            }
            else if (key.Key is RsaSecurityKey rsaKey)
            {
                var parameters = rsaKey.Rsa?.ExportParameters(false) ?? rsaKey.Parameters;
                var exponent = Base64Url.Encode(parameters.Exponent);
                var modulus = Base64Url.Encode(parameters.Modulus);

                var webKey = new IdentityModel.Jwk.JsonWebKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = rsaKey.KeyId,
                    E = exponent,
                    N = modulus,
                    Alg = key.SigningAlgorithm
                };

                webKeys.Add(webKey);
            }
            else if (key.Key is ECDsaSecurityKey ecdsaKey)
            {
                var parameters = ecdsaKey.ECDsa.ExportParameters(false);
                var x = Base64Url.Encode(parameters.Q.X);
                var y = Base64Url.Encode(parameters.Q.Y);

                var ecdsaJsonWebKey = new IdentityModel.Jwk.JsonWebKey
                {
                    Kty = "EC",
                    Use = "sig",
                    Kid = ecdsaKey.KeyId,
                    X = x,
                    Y = y,
                    Crv = CryptoHelper.GetCrvValueFromCurve(parameters.Curve),
                    Alg = key.SigningAlgorithm
                };
                webKeys.Add(ecdsaJsonWebKey);
            }
            else if (key.Key is JsonWebKey jwk)
            {
                var webKey = new IdentityModel.Jwk.JsonWebKey
                {
                    Kty = jwk.Kty,
                    Use = jwk.Use ?? "sig",
                    Kid = jwk.Kid,
                    X5t = jwk.X5t,
                    E = jwk.E,
                    N = jwk.N,
                    X5c = 0 == jwk.X5c?.Count ? null : jwk.X5c.ToArray(),
                    Alg = jwk.Alg,
                    Crv = jwk.Crv,
                    X = jwk.X,
                    Y = jwk.Y
                };

                webKeys.Add(webKey);
            }
        }

        return webKeys;
    }
}