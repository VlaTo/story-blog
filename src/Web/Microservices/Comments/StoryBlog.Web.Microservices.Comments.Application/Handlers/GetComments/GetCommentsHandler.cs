﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Domain.Specifications;
using StoryBlog.Web.Microservices.Comments.Application.Models;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComments;

public sealed class GetCommentsHandler : HandlerBase, IRequestHandler<GetCommentsQuery, GetCommentsResult>
{
    private readonly IUnitOfWork context;
    private readonly IMapper mapper;
    private readonly ILogger<GetCommentsHandler> logger;

    public GetCommentsHandler(IUnitOfWork context, IMapper mapper, ILogger<GetCommentsHandler> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<GetCommentsResult> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<Domain.Entities.Comment>? comments;
        ISpecification<Domain.Entities.Comment> specification = null == request.ParentKey
            ? new FindRootCommentsForPost(request.PostKey, request.PageNumber, request.PageSize)
            : new FindChildrenCommentsForPost(request.PostKey, request.ParentKey!.Value);

        await using (var repository = context.GetRepository<Domain.Entities.Comment>())
        {
            comments = await repository.QueryAsync(specification, cancellationToken);
        }

        return new GetCommentsResult(
            mapper.Map<IReadOnlyList<Comment>>(comments, options =>
            {
                ;
            })
        );
    }
}