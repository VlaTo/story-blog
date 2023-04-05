namespace StoryBlog.Web.Client.Blog.Clients;

internal class HttpClientBase
{
    protected HttpClient Client
    {
        get;
    }

    protected HttpClientBase(HttpClient client)
    {
        Client = client;
    }
}