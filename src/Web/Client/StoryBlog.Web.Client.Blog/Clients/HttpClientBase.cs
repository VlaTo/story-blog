namespace StoryBlog.Web.Client.Blog.Clients;

internal class HttpClientBase
{
    public HttpClient HttpClient
    {
        get;
    }

    protected HttpClientBase(HttpClient httpClient)
    {
        HttpClient = httpClient;
    }
}