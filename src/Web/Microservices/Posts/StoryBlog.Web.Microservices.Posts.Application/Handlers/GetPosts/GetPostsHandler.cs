using MediatR;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;
using StoryBlog.Web.Common.Identity.Permission;
using Post = StoryBlog.Web.Microservices.Posts.Domain.Entities.Post;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPosts;

public sealed class GetPostsHandler : HandlerBase, IRequestHandler<GetPostsQuery, Result<(IReadOnlyList<Brief> Posts, int PageNumber, int PageSize, int PagesCount)>>
{
    private readonly IAsyncUnitOfWork context;

    public GetPostsHandler(IAsyncUnitOfWork context)
    {
        this.context = context;
    }

    public async Task<Result<(IReadOnlyList<Brief> Posts, int PageNumber, int PageSize, int PagesCount)>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var authenticated = request.CurrentUser.IsAuthenticated();

        if (authenticated)
        {
            if (false == request.CurrentUser.HasPermission(Permissions.Blogs.View))
            {
                return new Exception("Insufficient permissions");
            }

            var userId = request.CurrentUser.GetSubject();

            if (String.IsNullOrEmpty(userId))
            {
                return new Exception("No user identity");
            }
        }
        else
        {
            return new Exception("User not authenticated");
        }

        await using (var repository = context.GetRepository<Post>())
        {
            ISpecification<Post> specification = new AllPostsSpecification(authenticated);
            var postsCount = await repository.CountAsync(specification, cancellationToken);

            specification = new PagedPostsSpecification(authenticated, request.PageNumber, request.PageSize);
            var posts = await repository.QueryAsync(specification, cancellationToken);
            
            var briefs = new Brief[posts.Length];

            for (var index = 0; index < posts.Length; index++)
            {
                var source = posts[index];

                briefs[index] = new Brief
                {
                    Key = source.Key,
                    Title = source.Title,
                    Slug = source.Slug.Text,
                    Status = source.Status,
                    Author = source.AuthorId,
                    Text = source.Content.Brief!,
                    CommentsCount = source.CommentsCounter.Counter,
                    IsPublic = source.IsPublic,
                    AllowedActions = AllowedActions.CanTogglePublic | AllowedActions.CanEdit | AllowedActions.CanDelete,
                    CreatedAt = source.CreateAt
                };
            }

            var (quotient, remainder) = Math.DivRem(postsCount, request.PageSize);
            var pagesCount = quotient + (remainder > 0 ? 1 : 0);

            return (Posts: briefs, request.PageNumber, request.PageSize, PagesCount: pagesCount);
        }
    }
}