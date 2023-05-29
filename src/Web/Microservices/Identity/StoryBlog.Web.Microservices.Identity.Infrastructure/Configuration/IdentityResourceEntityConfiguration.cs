using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Domain;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Configuration;

internal sealed class IdentityResourceEntityConfiguration : IEntityTypeConfiguration<IdentityResource>
{
    private readonly ConfigurationStoreOptions storeOptions;

    public IdentityResourceEntityConfiguration(ConfigurationStoreOptions storeOptions)
    {
        this.storeOptions = storeOptions;
    }

    public void Configure(EntityTypeBuilder<IdentityResource> builder)
    {
        builder
            .Property(x => x.Id)
            .IsRequired(required: true);
        builder
            .Property(x => x.Enabled)
            .HasDefaultValue(true)
            .IsRequired(required: true);
        builder
            .Property(x => x.Name)
            .IsRequired(required: true)
            .HasMaxLength(1024)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.DisplayName)
            .IsRequired(required: true)
            .HasMaxLength(1024)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.Description)
            .IsRequired(required: false)
            .HasMaxLength(2048)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.Required)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.Emphasize)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.ShowInDiscoveryDocument)
            .IsRequired(required: true)
            .HasDefaultValue(true);
        builder
            .Property(x => x.Created)
            .IsRequired(required: true)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnAdd();
        builder
            .Property(x => x.Updated)
            .IsRequired(required: false)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnUpdate();
        builder
            .Property(x => x.NonEditable)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .OwnsMany(
                x => x.UserClaims,
                claims =>
                {
                    claims
                        .Property(x => x.Type)
                        .HasMaxLength(256)
                        .IsRequired(required: true);

                    claims
                        .WithOwner(x => x.IdentityResource)
                        .HasForeignKey(x => x.IdentityResourceId);

                    claims
                        .HasKey(x => x.Id);

                    claims
                        .ToTable(TableNames.IdentityResourceClaim, SchemaNames.Identity);
                });
        builder
            .OwnsMany(
                x => x.Properties,
                properties =>
                {
                    properties
                        .Property(x => x.Key)
                        .HasMaxLength(256)
                        .IsRequired(required: true);
                    properties
                        .Property(x => x.Value)
                        .HasMaxLength(Int16.MaxValue)
                        .IsRequired(required: true);

                    properties
                        .WithOwner(x => x.IdentityResource)
                        .HasForeignKey(x => x.IdentityResourceId);

                    properties
                        .HasKey(x => x.Id);

                    properties
                        .ToTable(TableNames.IdentityResourceProperty, SchemaNames.Identity);
                });

        builder
            .HasKey(x => x.Id);

        builder
            .ToTable(TableNames.IdentityResource, SchemaNames.Identity);
    }
}