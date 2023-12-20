using AutoMapper;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;

public class GetPostHandler : PostHandlerBase, MediatR.IRequestHandler<GetPostQuery, Result<Models.Post>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMapper mapper;

    public GetPostHandler(
        IAsyncUnitOfWork context,
        IMapper mapper,
        ILogger<GetPostHandler> logger)
        : base(logger)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Result<Models.Post>> Handle(GetPostQuery request, CancellationToken cancellationToken)
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
            var entity = await repository.FindPostBySlugOrKeyAsync(request.SlugOrKey, cancellationToken: cancellationToken);

            if (null != entity)
            {
                var post = mapper.Map<Models.Post>(entity);
                return post;
            }
        }

        return new Exception("No post found");
    }
}