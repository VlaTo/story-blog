namespace StoryBlog.Web.Client.Blog.Clients;

internal class HttpClientBase
{
    protected HttpClient ClientFactory
    {
        get;
    }

    protected HttpClientBase(HttpClient clientFactory)
    {
        ClientFactory = clientFactory;
    }
}