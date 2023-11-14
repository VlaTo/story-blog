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
            .IsUnicode(unicode: true)
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
                    .IsUnicode(unicode: true)
                    .IsRequired();

                content
                    .Property(x => x.Brief)
                    .HasMaxLength(1024)
                    .IsUnicode(unicode: true)
                    .IsRequired();

                content.ToTable("Contents", "Blog");
            });

        builder.Property(x => x.IsPublic);
        
        builder.Property(x => x.Status);

        builder.Property(x => x.AuthorId)
            .IsRequired(required: true)
            .HasMaxLength(150);

        builder.Property(x => x.CreateAt)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ModifiedAt)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
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
                    .HasMaxLength(250)
                    .IsUnicode(unicode: true)
                    .IsRequired();

                slug
                    .HasIndex(x => x.Text)
                    .IsUnique(unique: true);

                slug.ToTable("Slugs", "Blog");
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

                counter.ToTable("CommentsCounters", "Blog");
            });

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Key).IsUnique(unique: true);
        builder.HasIndex(x => x.Title);

        builder.ToTable("Posts", "Blog");
    }
}