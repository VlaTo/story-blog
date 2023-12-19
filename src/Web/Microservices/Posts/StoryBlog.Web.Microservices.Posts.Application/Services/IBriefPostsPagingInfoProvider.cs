namespace StoryBlog.Web.Microservices.Posts.Application.Services;

public interface IBriefPostsPagingInfoProvider
{
    string GeneratePagingInfo(int pageNumber, int pageSize);
}