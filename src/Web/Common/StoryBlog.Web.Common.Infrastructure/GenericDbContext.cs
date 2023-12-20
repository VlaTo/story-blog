using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Common.Application;

namespace StoryBlog.Web.Common.Infrastructure;

public abstract class GenericDbContext : DbContext, IGenericDbContext
{
    protected GenericDbContext()
    {
    }

    protected GenericDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public virtual bool HasChanges() => ChangeTracker.HasChanges();

    public abstract Task RollbackAsync(CancellationToken cancellationToken = default);

    /*
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        GenerateOnUpdate();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        GenerateOnUpdate();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void GenerateOnUpdate()
    {
        foreach (var entityEntry in ChangeTracker.Entries())
        {
            foreach (var propertyEntry in entityEntry.Properties)
            {
                var property = propertyEntry.Metadata;
                var valueGeneratorFactory = property.GetValueGeneratorFactory();
                var generatedOnUpdate = (property.ValueGenerated & ValueGenerated.OnUpdate) == ValueGenerated.OnUpdate;

                if (false == generatedOnUpdate || null == valueGeneratorFactory)
                {
                    continue;
                }

                var valueGenerator = valueGeneratorFactory.Invoke(property, entityEntry.Metadata);
                propertyEntry.CurrentValue = valueGenerator.Next(entityEntry);
                propertyEntry.IsModified = true;
            }
        }
    }
    */
}