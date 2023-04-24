using MediatR;
using StoryBlog.Web.Common.Application;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.GetComment;

public sealed class GetCommentHandler : HandlerBase, IRequestHandler<GetCommentQuery, GetCommentResult>
{
    public GetCommentHandler()
    {
    }

    public Task<GetCommentResult> Handle(GetCommentQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}