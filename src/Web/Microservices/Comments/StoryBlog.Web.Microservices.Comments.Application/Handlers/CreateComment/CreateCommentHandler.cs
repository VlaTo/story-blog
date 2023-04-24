using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Common.Application;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Application.Handlers.CreateComment;

public sealed class CreateCommentHandler : HandlerBase, IRequestHandler<CreateCommentCommand, Guid?>
{
    private readonly IUnitOfWork context;
    private readonly ILogger<CreateCommentHandler> logger;

    public CreateCommentHandler(IUnitOfWork context, ILogger<CreateCommentHandler> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public Task<Guid?> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        if(request.)
        var comment=new Comment
        {

        }
    }
}