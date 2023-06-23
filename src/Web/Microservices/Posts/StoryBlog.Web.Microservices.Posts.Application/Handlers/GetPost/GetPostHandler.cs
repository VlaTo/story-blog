using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;

public class GetPostHandler : HandlerBase, IRequestHandler<GetPostQuery, Result<Models.Post>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMapper mapper;
    private readonly ILogger<GetPostHandler> logger;

    public GetPostHandler(
        IAsyncUnitOfWork context,
        IMapper mapper,
        ILogger<GetPostHandler> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<Result<Models.Post>> Handle(GetPostQuery request, CancellationToken cancellationToken)
    {
        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var specification = new FindPostByKeySpecification(request.Key);
            var entity = await repository
                .FindAsync(specification, cancellationToken)
                .ConfigureAwait(false);

            if (null != entity)
            {
                var post = mapper.Map<Models.Post>(entity);
                return post;
            }
        }

        return new Exception("No post found");
    }
}