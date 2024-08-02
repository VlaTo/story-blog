using StoryBlog.Web.Identity.Client.Extensions;

namespace StoryBlog.Web.Identity.Client;

public sealed record DiscoveryEndpoint(string Authority, string Url)
{
    public static DiscoveryEndpoint ParseUrl(string url, string? path = null)
    {
        if (null == url)
        {
            throw new ArgumentNullException(nameof(url));
        }

        if (String.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("input is empty", nameof(url));
        }

        if (String.IsNullOrEmpty(path))
        {
            path = OidcConstants.Discovery.DiscoveryEndpoint;
        }

        var success = Uri.TryCreate(url, UriKind.Absolute, out var uri);

        if (false == success)
        {
            throw new InvalidOperationException("Malformed URL");
        }

        if (false == IsValidScheme(uri!))
        {
            throw new InvalidOperationException("Malformed URL");
        }

        url = url.RemoveTrailingSlash()!;

        if (path.StartsWith('/'))
        {
            path = path.Substring(1);
        }

        return url.EndsWith(path, StringComparison.OrdinalIgnoreCase)
            ? new DiscoveryEndpoint(url.Substring(0, url.Length - path.Length - 1), url)
            : new DiscoveryEndpoint(url, url.EnsureTrailingSlash() + path);
    }

    public static bool IsValidScheme(Uri url)
    {
        var actualScheme = url.Scheme;
        string[] validSchemes = [Uri.UriSchemeHttp, Uri.UriSchemeHttps];
        return validSchemes.Any(scheme => String.Equals(actualScheme, scheme, StringComparison.OrdinalIgnoreCase));
    }
    
    public static bool IsSecureScheme(Uri url, DiscoveryPolicy policy)
    {
        if (policy.RequireHttps)
        {
            if (policy.AllowHttpOnLoopback)
            {
                var hostName = url.DnsSafeHost;

                foreach (var address in policy.LoopbackAddresses)
                {
                    if (String.Equals(hostName, address, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return String.Equals(url.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase);
        }

        return true;
    }
}