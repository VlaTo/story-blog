namespace StoryBlog.Web.Identity.Client.Requests;

public class DiscoveryDocumentRequest : ProtocolRequest
{
    public DiscoveryPolicy Policy { get; set; } = new();

    public DiscoveryDocumentRequest(string? address)
    {
        Address = address;
    }

    public DiscoveryDocumentRequest(ProtocolRequest request)
        : base(request)
    {
    }
}