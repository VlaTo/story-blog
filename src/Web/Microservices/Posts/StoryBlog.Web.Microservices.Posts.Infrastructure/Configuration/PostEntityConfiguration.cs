using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Posts.Domain.Entities;

namespace StoryBlog.Web.Microservices.Posts.Infrastructure.Configuration;

internal sealed class PostEntityConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Key)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.Title)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(x => x.IsPublic);
        
        builder.Property(x => x.Status);

        builder.Property(x => x.CreateAt)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ModifiedAt)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnUpdate();

        builder.Property(x => x.DeletedAt);

        builder.OwnsOne(
            x => x.Slug,
            slug =>
            {
                slug
                    .WithOwner(x => x.Post)
                    .HasForeignKey(x => x.PostId);

                slug.Property(x => x.Text)
                    .HasMaxLength(200)
                    .IsRequired();

                slug.HasIndex(x => x.Text).IsUnique(unique: true);

                slug.ToTable("Slugs");
            });

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Key).IsUnique(unique: true);
        builder.HasIndex(x => x.Title);

        builder.ToTable("Posts");
    }
}