using AutoMapper;
using MediatR;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPosts;

public sealed class GetPostsHandler : HandlerBase, IRequestHandler<GetPostsQuery, GetPostsResult>
{
    private readonly IUnitOfWork context;
    private readonly IMapper mapper;

    public GetPostsHandler(IUnitOfWork context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<GetPostsResult> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.Post> posts;

        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var specification = new AllAvailablePostsSpecification(request.PageNumber, request.PageSize);
            posts = await repository.QueryAsync(specification, cancellationToken);
        }

        return new GetPostsResult(
            mapper.Map<IReadOnlyList<Post>>(posts, options =>
            {
                ;
            })
        );
    }
}