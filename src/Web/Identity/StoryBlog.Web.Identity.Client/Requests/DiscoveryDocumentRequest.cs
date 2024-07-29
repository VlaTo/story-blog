namespace StoryBlog.Web.Identity.Client.Requests;

public class DiscoveryDocumentRequest : ProtocolRequest
{
    public string? Address
    {
        get;
    }

    public DiscoveryDocumentRequest(string? address)
    {
        Address = address;
    }
}