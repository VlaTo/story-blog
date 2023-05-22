using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Application.Storage;

namespace StoryBlog.Web.Microservices.Identity.Application.Configuration;

internal sealed class ConfigureClients : IConfigureOptions<ApiAuthorizationOptions>
{
    private const string DefaultLocalSPARelativeRedirectUri = "/authentication/login-callback";
    private const string DefaultLocalSPARelativePostLogoutRedirectUri = "/authentication/logout-callback";

    private readonly IConfiguration _configuration;
    private readonly ILogger<ConfigureClients> _logger;

    public ConfigureClients(IConfiguration configuration, ILogger<ConfigureClients> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void Configure(ApiAuthorizationOptions options)
    {
        foreach (var client in GetClients())
        {
            options.Clients.Add(client);
        }
    }

    internal IEnumerable<Client> GetClients()
    {
        var data = _configuration.Get<Dictionary<string, ClientDefinition>>();

        if (null == data)
        {
            yield break;
        }

        foreach (var (name, definition) in data)
        {
            _logger.LogInformation(LoggerEventIds.ConfiguringClient, "Configuring client '{ClientName}'.", name);

            yield return definition.Profile switch
            {
                ApplicationProfiles.SPA => GetSPA(name, definition),
                ApplicationProfiles.IdentityServerSPA => GetLocalSPA(name, definition),
                ApplicationProfiles.NativeApp => GetNativeApp(name, definition),
                _ => throw new InvalidOperationException($"Type '{definition.Profile}' is not supported.")
            };
        }
    }

    private static Client GetSPA(string name, ClientDefinition definition)
    {
        if (null == definition.RedirectUri ||
            false == Uri.TryCreate(definition.RedirectUri, UriKind.Absolute, out var redirectUri))
        {
            throw new InvalidOperationException($"The redirect uri " +
                $"'{definition.RedirectUri}' for '{name}' is invalid. " +
                $"The redirect URI must be an absolute url.");
        }

        if (null == definition.LogoutUri ||
            false == Uri.TryCreate(definition.LogoutUri, UriKind.Absolute, out var postLogoutUri))
        {
            throw new InvalidOperationException($"The logout uri " +
                $"'{definition.LogoutUri}' for '{name}' is invalid. " +
                $"The logout URI must be an absolute url.");
        }

        if (false == String.Equals(
            redirectUri.GetLeftPart(UriPartial.Authority),
            postLogoutUri.GetLeftPart(UriPartial.Authority),
            StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"The redirect uri and the logout uri " +
                $"for '{name}' have a different scheme, host or port.");
        }

        return ClientBuilder.SPA(name)
            .WithRedirectUri(definition.RedirectUri)
            .WithLogoutRedirectUri(definition.LogoutUri)
            .WithAllowedOrigins(redirectUri.GetLeftPart(UriPartial.Authority))
            .FromConfiguration()
            .Build();
    }

    private static Client GetNativeApp(string name, ClientDefinition definition) =>
        ClientBuilder
            .NativeApp(name)
            .FromConfiguration()
            .Build();

    private static Client GetLocalSPA(string name, ClientDefinition definition) =>
        ClientBuilder
            .IdentityServerSPA(name)
            .WithRedirectUri(definition.RedirectUri ?? DefaultLocalSPARelativeRedirectUri)
            .WithLogoutRedirectUri(definition.LogoutUri ?? DefaultLocalSPARelativePostLogoutRedirectUri)
            .WithAllowedOrigins()
            .FromConfiguration()
            .Build();
}