using AutoMapper;
using MediatR;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;
using System.Diagnostics;
using StoryBlog.Web.Common.Identity.Permission;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPosts;

public sealed class GetPostsHandler : HandlerBase, IRequestHandler<GetPostsQuery, Result<IReadOnlyList<Brief>>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMapper mapper;

    public GetPostsHandler(IAsyncUnitOfWork context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<Result<IReadOnlyList<Brief>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Post> posts;

        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var authenticated = request.CurrentUser.IsAuthenticated();

            if (authenticated && request.CurrentUser.IsInRole(Permissions.Blogs.View))
            {
                Debugger.Break();
            }

            var specification = new AllAvailablePostsSpecification(authenticated, request.PageNumber, request.PageSize);
            posts = await repository.QueryAsync(specification, cancellationToken);
        }

        return new Result<IReadOnlyList<Brief>>(
            mapper.Map<IReadOnlyList<Brief>>(posts, options =>
            {
                ;
            })
        );
    }
}