using MediatR;
using StoryBlog.Web.Common.Result;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GenerateSlug;

public sealed record GenerateSlugQuery(string Title) : IRequest<Result<string>>;