using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Comments.Domain.Entities;

namespace StoryBlog.Web.Microservices.Comments.Infrastructure.Configuration;

internal sealed class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Key)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(x => x.ParentId)
            .IsRequired(false);

        builder.Property(x => x.PostKey)
            .IsRequired();

        builder.Property(x => x.Text)
            .HasMaxLength(1024)
            .IsUnicode(unicode: true)
            .IsRequired();

        builder.Property(x => x.IsPublic);

        builder.Property(x => x.Status);
        
        builder.Property(x => x.CreateAt)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
            .ValueGeneratedOnAdd();

        builder.Property(x => x.ModifiedAt)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
            .ValueGeneratedOnUpdate();

        builder.Property(x => x.DeletedAt);

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.PostKey);
        builder.HasIndex(x => x.ParentId);

        builder
            .HasOne(x => x.Parent)
            .WithMany(x => x.Comments)
            .HasForeignKey(x => x.ParentId)
            .HasPrincipalKey(x => x.Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder.ToTable("Comments", "Comment");
    }
}