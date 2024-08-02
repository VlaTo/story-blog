using StoryBlog.Web.Identity.Client.Extensions;

namespace StoryBlog.Web.Identity.Client;

public sealed class AuthorityUrlValidationStrategy : IAuthorityValidationStrategy
{
    public AuthorityValidationResult IsIssuerNameValid(string issuerName, string expectedAuthority)
    {
        if (false == Uri.TryCreate(expectedAuthority.RemoveTrailingSlash(), UriKind.Absolute, out var expectedAuthorityUrl))
        {
            throw new ArgumentOutOfRangeException("Authority must be a valid URL.", nameof(expectedAuthority));
        }

        if (String.IsNullOrWhiteSpace(issuerName))
        {
            return AuthorityValidationResult.CreateError("Issuer name is missing");
        }

        if (false == Uri.TryCreate(issuerName.RemoveTrailingSlash(), UriKind.Absolute, out var issuerUrl))
        {
            return AuthorityValidationResult.CreateError("Issuer name is not a valid URL");
        }

        if (String.Equals(expectedAuthorityUrl, issuerUrl))
        {
            return AuthorityValidationResult.SuccessResult;
        }

        return AuthorityValidationResult.CreateError("Issuer name does not match authority: " + issuerName);
    }

    public AuthorityValidationResult IsEndpointValid(string endpoint, IEnumerable<string> allowedAuthorities)
    {
        if (String.IsNullOrEmpty(endpoint))
        {
            return AuthorityValidationResult.CreateError("endpoint is empty");
        }

        if (false == Uri.TryCreate(endpoint.RemoveTrailingSlash(), UriKind.Absolute, out var endpointUrl))
        {
            return AuthorityValidationResult.CreateError("Endpoint is not a valid URL");
        }

        foreach (var authority in allowedAuthorities)
        {
            if (false == Uri.TryCreate(authority.RemoveTrailingSlash(), UriKind.Absolute, out var authorityUrl))
            {
                throw new ArgumentOutOfRangeException("Authority must be a URL.", nameof(allowedAuthorities));
            }

            var expectedString = authorityUrl.ToString();
            var testString = endpointUrl.ToString();

            if (testString.StartsWith(expectedString, StringComparison.Ordinal))
            {
                return AuthorityValidationResult.SuccessResult;
            }

        }

        var expectedBaseAddresses = String.Join(",", allowedAuthorities);
        
        return AuthorityValidationResult.CreateError($"Invalid base address for endpoint {endpoint}. Valid base addresses: {expectedBaseAddresses}.");
    }
}