using HashidsNet;

namespace StoryBlog.Web.Microservices.Posts.Application.Services;

internal sealed class BriefPostsPagingInfoProvider : IBriefPostsPagingInfoProvider
{
    private readonly Hashids hashids;

    public BriefPostsPagingInfoProvider()
    {
        hashids = new Hashids();
    }

    public string GeneratePagingInfo(int pageNumber, int pageSize)
    {
        return hashids.Encode(pageNumber, pageSize);
    }
}