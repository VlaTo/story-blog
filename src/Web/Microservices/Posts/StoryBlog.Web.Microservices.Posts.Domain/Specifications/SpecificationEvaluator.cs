using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Domain.Specifications;

public class SpecificationEvaluator<TEntity> where TEntity : class, IEntity
{
    public static IQueryable<TEntity> Query(IQueryable<TEntity> entities, ISpecification<TEntity> specification)
    {
        var query = entities.AsQueryable();

        if (null != specification.Criteria)
        {
            query = query.Where(specification.Criteria);
        }

        if (0 < specification.Includes.Count)
        {
            
            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));
        }

        if (0 < specification.OrderBy.Count)
        {
            var ordered = query.OrderBy(specification.OrderBy[0]);

            for (var index = 1; index < specification.OrderBy.Count; index++)
            {
                ordered = ordered.ThenBy(specification.OrderBy[index]);
            }

            query = ordered;
        }

        if (null != specification.Skip)
        {
            query = query.Skip(specification.Skip.Value);
        }

        if (null != specification.Take)
        {
            query = query.Take(specification.Take.Value);
        }

        return query;
    }
}