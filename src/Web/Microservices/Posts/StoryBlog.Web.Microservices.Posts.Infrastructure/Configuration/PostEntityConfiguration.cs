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

        builder.OwnsOne(
            x => x.Content,
            content =>
            {
                content
                    .WithOwner(x => x.Post)
                    .HasForeignKey(x => x.PostId);

                content
                    .Property(x => x.Text)
                    .HasColumnType("ntext")
                    .IsRequired();

                content
                    .Property(x => x.Brief)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(1024)
                    .IsRequired();

                content.ToTable("Contents");
            });

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

                slug
                    .Property(x => x.Text)
                    .HasMaxLength(200)
                    .IsRequired();

                slug
                    .HasIndex(x => x.Text)
                    .IsUnique(unique: true);

                slug.ToTable("Slugs");
            });

        builder.OwnsOne(
            x => x.CommentsCounter,
            counter =>
            {
                counter
                    .WithOwner(x => x.Post)
                    .HasForeignKey(x => x.PostId);

                counter.Property(x => x.Counter)
                    .IsRequired();

                counter
                    .HasIndex(x => x.PostId)
                    .IsUnique(unique: true);

                counter.ToTable("CommentsCounters");
            });

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Key).IsUnique(unique: true);
        builder.HasIndex(x => x.Title);

        builder.ToTable("Posts");
    }
}