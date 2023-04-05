using System.Linq.Expressions;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public interface ISpecification<TEntity> where TEntity : IEntity
{
    Expression<Func<TEntity, bool>>? Criteria
    {
        get;
    }

    IList<Expression<Func<TEntity, object?>>> Includes
    {
        get;
    }
    
    IList<Expression<Func<TEntity, object?>>> OrderBy
    {
        get;
    }

    int? Skip
    {
        get;
    }

    int? Take
    {
        get;
    }
}