namespace StoryBlog.Web.Identity.Client;

public interface IAuthorityValidationStrategy
{
    AuthorityValidationResult IsIssuerNameValid(string issuerName, string expectedAuthority);

    AuthorityValidationResult IsEndpointValid(string endpoint, IEnumerable<string> expectedAuthority);
}