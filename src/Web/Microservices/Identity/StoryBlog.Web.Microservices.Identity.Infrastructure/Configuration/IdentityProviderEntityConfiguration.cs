using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Domain;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Configuration;

internal sealed class IdentityProviderEntityConfiguration : IEntityTypeConfiguration<IdentityProvider>
{
    private readonly ConfigurationStoreOptions storeOptions;

    public IdentityProviderEntityConfiguration(ConfigurationStoreOptions storeOptions)
    {
        this.storeOptions = storeOptions;
    }

    public void Configure(EntityTypeBuilder<IdentityProvider> builder)
    {
        builder
            .Property(x => x.Type)
            .IsRequired(required: true)
            .HasMaxLength(256)
            .IsUnicode(unicode: false);
        builder
            .Property(x => x.DisplayName)
            .IsRequired(required: false)
            .HasMaxLength(Int16.MaxValue)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.Scheme)
            .IsRequired(required: true)
            .HasMaxLength(256);
        builder
            .Property(x => x.Enabled)
            .IsRequired(required: true)
            .HasDefaultValue(true);
        builder
            .Property(x => x.Properties)
            .IsRequired(required: true)
            .HasMaxLength(Int16.MaxValue)
            .IsUnicode(unicode: false);
        builder
            .Property(x => x.Created)
            .IsRequired(required: true)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
            .ValueGeneratedOnAdd();
        builder
            .Property(x => x.LastAccessed)
            .IsRequired(required: true);
        builder
            .Property(x => x.Updated)
            .IsRequired(required: false)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
            .ValueGeneratedOnUpdate();
        builder
            .Property(x => x.NonEditable)
            .IsRequired(required: true)
            .HasDefaultValue(false);

        builder
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Type);

        builder
            .ToTable(TableNames.IdentityProvider, SchemaNames.Identity);
    }
}