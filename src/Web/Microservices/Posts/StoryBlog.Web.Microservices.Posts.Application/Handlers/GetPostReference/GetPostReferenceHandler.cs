﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPostReference;

public sealed class GetPostReferenceHandler : HandlerBase, IRequestHandler<GetPostReferenceQuery, Result<PostReference>>
{
    private readonly IAsyncUnitOfWork context;
    private readonly IMapper mapper;
    private readonly ILogger<GetPostReferenceHandler> logger;

    public GetPostReferenceHandler(
        IAsyncUnitOfWork context,
        IMapper mapper,
        ILogger<GetPostReferenceHandler> logger)
    {
        this.context = context;
        this.mapper = mapper;
        this.logger = logger;
    }

    public async Task<Result<PostReference>> Handle(GetPostReferenceQuery request, CancellationToken cancellationToken)
    {
        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var specification = new FindPostBySlugSpecification(request.Slug, includeAll: false);
            var entity = await repository
                .FindAsync(specification, cancellationToken)
                .ConfigureAwait(false);

            if (null != entity)
            {
                var post = mapper.Map<PostReference>(entity);
                return post;
            }
        }

        return new Exception("");
    }
}