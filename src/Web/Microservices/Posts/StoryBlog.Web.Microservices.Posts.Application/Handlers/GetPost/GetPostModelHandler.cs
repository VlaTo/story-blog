using AutoMapper;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Common.Identity.Permission;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Extensions;
using StoryBlog.Web.Microservices.Posts.Application.Models;
using StoryBlog.Web.Microservices.Posts.Domain.Specifications;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPost;

public sealed class GetPostModelHandler(
    IAsyncUnitOfWork context,
    IMapper mapper,
    ILogger<GetPostModelHandler> logger)
    : PostHandlerBase(logger),
    MediatR.IRequestHandler<GetPostModelQuery, Result<Post>>
{
    public async Task<Result<Post>> Handle(GetPostModelQuery request, CancellationToken cancellationToken)
    {
        if (false == request.Principal.TryGetClientId(out var clientId))
        {
            return new Exception();
        }

        if (false == request.Principal.ClientHasPermission(Permissions.Blogs.View))
        {
            return new Exception();
        }

        await using (var repository = context.GetRepository<Domain.Entities.Post>())
        {
            var specification = new FindPostByKeySpecification(request.PostKey, includeAll: true);
            var entity = await repository.FindAsync(specification, cancellationToken: cancellationToken);

            if (null != entity)
            {
                var post = mapper.Map<Post>(entity);
                return post;
            }
        }

        return new Exception();
    }
}