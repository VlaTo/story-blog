using StoryBlog.Web.Identity.Client.Extensions;

namespace StoryBlog.Web.Identity.Client;

public class StringComparisonAuthorityValidationStrategy(StringComparison stringComparison) : IAuthorityValidationStrategy
{
    public AuthorityValidationResult IsIssuerNameValid(string issuerName, string expectedAuthority)
    {
        if (String.IsNullOrWhiteSpace(issuerName))
        {
            return AuthorityValidationResult.CreateError("Issuer name is missing");
        }

        if (String.Equals(issuerName.RemoveTrailingSlash(), expectedAuthority.RemoveTrailingSlash(), stringComparison))
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

        foreach (var authority in allowedAuthorities)
        {
            if (endpoint.StartsWith(authority, stringComparison))
            {
                return AuthorityValidationResult.SuccessResult;
            }
        }

        var expectedBaseAddresses = String.Join(',', allowedAuthorities);

        return AuthorityValidationResult.CreateError($"Invalid base address for endpoint {endpoint}. Valid base addresses: {expectedBaseAddresses}.");
    }
}