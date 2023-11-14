using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Configuration;

internal sealed class KeyEntityConfiguration : IEntityTypeConfiguration<Key>
{
    private readonly ConfigurationStoreOptions storeOptions;

    public KeyEntityConfiguration(ConfigurationStoreOptions storeOptions)
    {
        this.storeOptions = storeOptions;
    }

    public void Configure(EntityTypeBuilder<Key> builder)
    {
        builder
            .Property(x => x.Id)
            .IsRequired(required: true);
        builder
            .Property(x => x.Algorithm)
            .HasMaxLength(1024)
            .IsRequired(required: true);
        builder
            .Property(x => x.Data)
            .HasMaxLength(Int16.MaxValue)
            .IsRequired(required: true)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.Use)
            .HasMaxLength(256)
            .IsRequired(required: true);
        builder
            .Property(x => x.IsX509Certificate)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.DataProtected)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.Created)
            .IsRequired(required: true)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
            .ValueGeneratedOnAdd();
        builder
            .Property(x => x.Version)
            .IsRequired(required: true)
            .HasDefaultValue(0);

        builder.HasKey(x => x.Id);

        builder.ToTable(storeOptions.Keys.Name, storeOptions.Keys.Schema);
    }
}