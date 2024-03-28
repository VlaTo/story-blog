using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Common.Domain.Entities;
using StoryBlog.Web.Common.Domain;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using StoryBlog.Web.Microservices.Comments.Domain.Specifications;
using StoryBlog.Web.Microservices.Comments.Shared.Models;
using TestProject1.Extensions;

namespace TestProject1.Controllers.Comments;

[TestClass]
public sealed class ListAllCommentsTests : TestComments
{
    private const int CommentsCount = 3;

    private static readonly Guid PostKey = Guid.Parse("{76d47ab2-8645-4fdd-a08f-4f040ffe6c43}");

    private static readonly List<Guid> CommentKeys = new();

    private IActionResult? Result
    {
        get;
        set;
    }

    #region Arrange / Act

    public override async Task ArrangeAsync()
    {
        await base.ArrangeAsync();

        await using (var uow = ActivatorUtilities.GetServiceOrCreateInstance<IAsyncUnitOfWork>(ServiceProvider))
        {
            await using (var repository = uow.GetRepository<Comment>())
            {
                for (var index = 0; index < CommentsCount; index++)
                {
                    var commentKey = Guid.NewGuid();
                    var comment = new Comment
                    {
                        Key = commentKey,
                        PostKey = PostKey,
                        PublicationStatus = PublicationStatus.Approved,
                        VisibilityStatus = VisibilityStatus.Public,
                        Text = $"test comment with ID: '{commentKey:B}'"
                    };

                    CommentKeys.Add(commentKey);

                    await repository.AddAsync(comment, CancellationToken.None);
                }

                await repository.SaveChangesAsync(CancellationToken.None);
            }
        }
    }

    public override async Task ActAsync()
    {
        Result = await Controller.ListAll(PostKey, parentKey: null, pageNumber: 1, pageSize: 10);
    }

    public override async Task CleanupAsync()
    {
        await using (var uow = ActivatorUtilities.GetServiceOrCreateInstance<IAsyncUnitOfWork>(ServiceProvider))
        {
            await using (var repository = uow.GetRepository<Comment>())
            {
                var entities = await repository.QueryAsync(new EmptySpecification(), CancellationToken.None);
                await repository.RemoveRangeAsync(entities, CancellationToken.None);
                await repository.SaveChangesAsync(CancellationToken.None);
            }
        }
    }

    #endregion

    [TestMethod]
    public void IsOkResult()
    {
        Assert.IsInstanceOfType<OkObjectResult>(Result);
    }

    [TestMethod]
    public void IsHttpSuccess()
    {
        Assert.IsInstanceOfType<OkObjectResult>(Result);
        Assert.AreEqual(StatusCodes.Status200OK, Result.AsResult<OkObjectResult>().StatusCode);
    }

    [TestMethod]
    public void HasListAllResponse()
    {
        Assert.IsInstanceOfType<ListAllResponse>(Result!.AsResult<OkObjectResult>().Value);
        Assert.AreEqual(CommentsCount, Result!.AsResult<OkObjectResult>().Value.As<ListAllResponse>()!.Comments.Count);
    }

    [TestMethod]
    public void HasAllCommentKeys()
    {
        var comments = Result!.AsResult<OkObjectResult>().Value.As<ListAllResponse>()!.Comments;
        
        foreach (var commentModel in comments)
        {
            Assert.AreEqual(PostKey, commentModel.PostKey);
            Assert.IsTrue(CommentKeys.Contains(commentModel.Key));
        }
    }

    [TestMethod]
    public async Task HasAllCommentsInDb()
    {
        var comments = Result!.AsResult<OkObjectResult>().Value.As<ListAllResponse>()!.Comments;

        await using (var uow = ActivatorUtilities.GetServiceOrCreateInstance<IAsyncUnitOfWork>(ServiceProvider))
        {
            await using (var repository = uow.GetRepository<Comment>())
            {
                foreach (var comment in comments)
                {
                    var entity = await repository.FindAsync(
                        new CommentOnlySpecification(comment.Key),
                        CancellationToken.None
                    );
                    Assert.AreEqual(PostKey, entity.PostKey);
                    Assert.AreEqual(comment.Key, entity.Key);
                    Assert.AreEqual(comment.AuthorId, entity.AuthorId);
                }
            }
        }
    }
}