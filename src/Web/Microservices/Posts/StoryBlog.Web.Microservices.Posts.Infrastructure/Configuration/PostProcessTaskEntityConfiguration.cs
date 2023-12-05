using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Configuration;

internal sealed class PostProcessTaskEntityConfiguration : IEntityTypeConfiguration<PostProcessTask>
{
    public void Configure(EntityTypeBuilder<PostProcessTask> builder)
    {
        builder.Property(x => x.Id)
            .IsRequired(required: true);

        builder.Property(x => x.Key)
            .IsRequired(required: true);

        builder.Property(x => x.PostKey)
            .IsRequired(required: true);

        builder.Property(x => x.Status)
            .IsRequired(required: true);

        builder.Property(x => x.Created)
            .IsRequired(required: true)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Completed)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
            .ValueGeneratedOnUpdate();
        
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Key)
            .IsUnique(unique: true);

        builder.ToTable("PostProcessTasks", "Blog");
    }
}