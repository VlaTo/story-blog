using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetTailPosts;

public sealed class GetTailPostsHandler : PostHandlerBase, IRequestHandler<GetTailPostsQuery, Result<IReadOnlyList<Brief>>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMapper mapper;

    public GetTailPostsHandler(
        IAsyncUnitOfWork context,
        IMapper mapper,
        ILogger<GetTailPostsHandler> logger)
        : base(logger)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<Brief>>> Handle(GetTailPostsQuery request, CancellationToken cancellationToken)
    {
        var authenticated = request.CurrentUser.IsAuthenticated();
        string? userId;

        if (authenticated)
        {
            if (false == request.CurrentUser.HasPermission(Permissions.Blogs.View))
            {
                return new Exception("Insufficient permissions");
            }

            userId = request.CurrentUser.GetSubject();

            if (String.IsNullOrEmpty(userId))
            {
                return new Exception("No user identity");
            }
        }
        else
        {
            return new Exception("User not authenticated");
        }

        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            ISpecification<Domain.Entities.Post> specification = new FindPostByKeySpecification(request.TailPostKey, false);
            var tailPost = await repository.FindAsync(specification, cancellationToken);

            if (null == tailPost)
            {
                return new Exception("Post not found");
            }

            specification = new TailPostsSpecification(authenticated, userId, tailPost.CreateAt, request.NumberOfPosts);
            
            var posts = await repository.QueryAsync(specification, cancellationToken);

            var briefs = mapper.Map<IReadOnlyList<Brief>>(posts, options => options.AfterMap((_, list) =>
            {
                for (var index = 0; index < list.Count; index++)
                {
                    list[index].AllowedActions = AllowedActions.CanTogglePublic | AllowedActions.CanEdit | AllowedActions.CanDelete;
                }
            }));

            /*var briefs = new Brief[posts.Length];

            for (var index = 0; index < posts.Length; index++)
            {
                var source = posts[index];

                briefs[index] = new Brief
                {
                    Key = source.Key,
                    Title = source.Title,
                    Slug = source.Slug.Text,
                    Status = source.PublicationStatus,
                    Author = source.AuthorId,
                    Text = source.Content.Brief,
                    CommentsCount = source.CommentsCounter.Counter,
                    AllowedActions = AllowedActions.CanEdit | AllowedActions.CanDelete,
                    CreatedAt = source.CreateAt
                };
            }*/

            return new Result<IReadOnlyList<Brief>>(briefs);
        }
    }
}