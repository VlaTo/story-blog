using AutoMapper;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result;
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
        await using (var repository = context.GetRepository<Post>())
        {
            var entity = await FindPostAsync(repository, request.SlugOrKey, cancellationToken);

            if (null != entity)
            {
                var post = mapper.Map<Models.Post>(entity);
                return post;
            }
        }

        return new Exception("No post found");
    }
}