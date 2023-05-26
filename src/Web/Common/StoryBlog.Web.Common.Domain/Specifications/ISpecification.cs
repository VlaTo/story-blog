using System.Linq.Expressions;
using StoryBlog.Web.Common.Domain.Entities;

namespace StoryBlog.Web.Common.Domain.Specifications;

[Flags]
public enum SpecificationQueryOptions
{
    NoTracking = 0x01,
    SplitQuery = 0x02
}

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

    SpecificationQueryOptions Options
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