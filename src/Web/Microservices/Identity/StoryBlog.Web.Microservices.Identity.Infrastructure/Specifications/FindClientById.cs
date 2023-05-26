using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Specifications;

internal sealed class FindClientById : SpecificationBase<Client>
{
    public FindClientById(string clientId)
    {
        Criteria = x => x.ClientId == clientId;
        Includes.Add(x => x.AllowedCorsOrigins);
        Includes.Add(x => x.AllowedGrantTypes);
        Includes.Add(x => x.AllowedScopes);
        Includes.Add(x => x.Claims);
        Includes.Add(x => x.ClientSecrets);
        Includes.Add(x => x.IdentityProviderRestrictions);
        Includes.Add(x => x.PostLogoutRedirectUris);
        Includes.Add(x => x.Properties);
        Includes.Add(x => x.RedirectUris);
        Options = (SpecificationQueryOptions.NoTracking | SpecificationQueryOptions.SplitQuery);
    }
}