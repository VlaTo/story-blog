using System.Security.Claims;
using MediatR;
using StoryBlog.Web.Common.Result;
using StoryBlog.Web.Microservices.Posts.Application.Models;

namespace StoryBlog.Web.Microservices.Posts.Application.Handlers.GetPostReference;

public sealed record GetPostReferenceQuery(string Slug, ClaimsPrincipal CurrentUser) : IRequest<Result<PostReference>>;