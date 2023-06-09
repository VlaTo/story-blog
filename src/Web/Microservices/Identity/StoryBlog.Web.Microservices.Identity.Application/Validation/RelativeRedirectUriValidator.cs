using StoryBlog.Web.Microservices.Identity.Application.Configuration;
using StoryBlog.Web.Microservices.Identity.Application.Core;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Application.Storage;

namespace StoryBlog.Web.Microservices.Identity.Application.Validation;

internal sealed class RelativeRedirectUriValidator : StrictRedirectUriValidator
{
    public IAbsoluteUrlFactory AbsoluteUrlFactory
    {
        get;
    }

    public RelativeRedirectUriValidator(IAbsoluteUrlFactory absoluteUrlFactory)
    {
        if (null == absoluteUrlFactory)
        {
            throw new ArgumentNullException(nameof(absoluteUrlFactory));
        }

        AbsoluteUrlFactory = absoluteUrlFactory;
    }

    public override Task<bool> IsRedirectUriValidAsync(string requestedUri, Client client)
    {
        if (IsLocalSPA(client))
        {
            return ValidateRelativeUris(requestedUri, client.RedirectUris);
        }

        return base.IsRedirectUriValidAsync(requestedUri, client);
    }

    public override Task<bool> IsPostLogoutRedirectUriValidAsync(string requestedUri, Client client)
    {
        if (IsLocalSPA(client))
        {
            return ValidateRelativeUris(requestedUri, client.PostLogoutRedirectUris);
        }

        return base.IsPostLogoutRedirectUriValidAsync(requestedUri, client);
    }

    private static bool IsLocalSPA(Client client) =>
        client.Properties.TryGetValue(ApplicationProfilesPropertyNames.Profile, out var clientType) &&
        ApplicationProfiles.IdentityServerSPA == clientType;

    private Task<bool> ValidateRelativeUris(string requestedUri, IEnumerable<string> clientUris)
    {
        foreach (var url in clientUris)
        {
            if (false == Uri.IsWellFormedUriString(url, UriKind.Relative))
            {
                continue;
            }

            var newUri = AbsoluteUrlFactory.GetAbsoluteUrl(url);

            if (String.Equals(newUri, requestedUri, StringComparison.Ordinal))
            {
                return Task.FromResult(true);
            }
        }

        return Task.FromResult(false);
    }
}