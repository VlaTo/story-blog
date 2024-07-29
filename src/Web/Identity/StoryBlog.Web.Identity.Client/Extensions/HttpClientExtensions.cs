using StoryBlog.Web.Identity.Client.Requests;
using StoryBlog.Web.Identity.Client.Responses;

namespace StoryBlog.Web.Identity.Client.Extensions;

public static class HttpClientExtensions
{
    public static async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(this HttpClient httpClient, string address, CancellationToken cancellationToken)
    {
        return await httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest(address), cancellationToken).ConfigureAwait(false);
    }
    
    public static async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(this HttpMessageInvoker invoker, DiscoveryDocumentRequest request, CancellationToken cancellationToken)
    {
        string address;

        if (false == String.IsNullOrEmpty(request.Address))
        {
            address = request.Address;
        }
        else if (invoker is HttpClient httpClient)
        {
            address = httpClient.BaseAddress!.AbsoluteUri;
        }
        else
        {
            throw new ArgumentNullException(nameof(address));
        }

        //var result = DiscoveryEndpoint.ParseUrl(address, request.Policy.DiscoveryDocumentPath);

        return new DiscoveryDocumentResponse();
    }
}