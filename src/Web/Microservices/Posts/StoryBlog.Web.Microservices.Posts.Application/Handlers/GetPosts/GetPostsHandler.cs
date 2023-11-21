﻿using MediatR;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Application.Extensions;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;
using StoryBlog.Web.Common.Identity.Permission;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPosts;

public sealed class GetPostsHandler : HandlerBase, IRequestHandler<GetPostsQuery, Result<IReadOnlyList<Brief>>>
{
    private readonly IAsyncUnitOfWork context;

    public GetPostsHandler(IAsyncUnitOfWork context)
    {
        this.context = context;
    }

    public async Task<Result<IReadOnlyList<Brief>>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var authenticated = request.CurrentUser.IsAuthenticated();

        if (authenticated)
        {
            if (false == request.CurrentUser.HasPermission(Permissions.Blogs.View))
            {
                return new Result<IReadOnlyList<Brief>>(new Exception("Insufficient permissions"));
            }

            var userId = request.CurrentUser.GetSubject();

            if (String.IsNullOrEmpty(userId))
            {
                return new Result<IReadOnlyList<Brief>>(new Exception("No user identity"));
            }
        }
        else
        {
            return new Result<IReadOnlyList<Brief>>(new Exception("User not authenticated"));
        }

        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var specification = new AllAvailablePostsSpecification(authenticated, request.PageNumber, request.PageSize);
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
                    Text = source.Content.Brief,
                    CommentsCount = source.CommentsCounter.Counter,
                    AllowedActions = AllowedActions.CanEdit | AllowedActions.CanDelete,
                    CreatedAt = source.CreateAt
                };
            }

            return briefs;
        }
    }
}