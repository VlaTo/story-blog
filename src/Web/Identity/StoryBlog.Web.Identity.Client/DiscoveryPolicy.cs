namespace StoryBlog.Web.Identity.Client;

public class DiscoveryPolicy
{
    internal static readonly IAuthorityValidationStrategy DefaultAuthorityValidationStrategy = new StringComparisonAuthorityValidationStrategy(StringComparison.Ordinal);

    public string Authority
    {
        get;
        set;
    } = String.Empty;

    public string? DiscoveryDocumentPath
    {
        get; 
        set;
    }

    public IAuthorityValidationStrategy AuthorityValidationStrategy
    {
        get; 
        set;
    } = DefaultAuthorityValidationStrategy;

    public bool RequireHttps
    {
        get; 
        set;
    } = true;

    public bool AllowHttpOnLoopback
    {
        get; 
        set;
    } = true;

    public ICollection<string> LoopbackAddresses = new HashSet<string>
    {
        "localhost", 
        "127.0.0.1"
    };

    public bool ValidateIssuerName
    {
        get; 
        set;
    } = true;

    public bool ValidateEndpoints
    {
        get; 
        set;
    } = true;

    public ICollection<string> EndpointValidationExcludeList
    {
        get;
        set;
    } = new HashSet<string>();

    public ICollection<string> AdditionalEndpointBaseAddresses
    {
        get;
        set;
    } = new HashSet<string>();

    public bool RequireKeySet
    {
        get; 
        set;
    } = true;
}