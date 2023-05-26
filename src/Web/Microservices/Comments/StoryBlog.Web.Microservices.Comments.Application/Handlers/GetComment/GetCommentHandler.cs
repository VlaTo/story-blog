using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComment;

public sealed class GetCommentHandler : HandlerBase, IRequestHandler<GetCommentQuery, Result<Models.Comment>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMapper mapper;
    private readonly ILogger<GetCommentHandler> logger;

    public GetCommentHandler(
        IAsyncUnitOfWork context,
        IMapper mapper,
        ILogger<GetCommentHandler> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<Result<Models.Comment>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        await using (var repository = context.GetRepository<Comment>())
        {
            var specification = new FindComment(request.Key);
            var comment = await repository.FindAsync(specification, cancellationToken: cancellationToken);

            if (null != comment)
            {
                return new Result<Models.Comment>(mapper.Map<Models.Comment>(comment));
            }
        }

        return new Result<Models.Comment>(new Exception("Comment not found"));
    }
}