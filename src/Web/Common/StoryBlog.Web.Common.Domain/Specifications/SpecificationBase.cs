using System.Linq.Expressions;
using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Common.Domain.Specifications;

public class SpecificationBase<TEntity> : ISpecification<TEntity> where TEntity : IEntity
{
    public Expression<Func<TEntity, bool>>? Criteria
    {
        get;
        protected set;
    }

    public IList<Expression<Func<TEntity, object?>>> Includes
    {
        get;
    }

    public IList<Expression<Func<TEntity, object?>>> OrderBy
    {
        get;
    }

    public int? Skip
    {
        get;
        protected set;
    }

    public int? Take
    {
        get;
        protected set;
    }

    public SpecificationBase()
    {
        Includes = new List<Expression<Func<TEntity, object?>>>();
        OrderBy = new List<Expression<Func<TEntity, object?>>>();
    }
}