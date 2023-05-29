using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Identity.Domain;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Configuration;

internal sealed class ApiResourceEntityConfiguration : IEntityTypeConfiguration<ApiResource>
{
    public void Configure(EntityTypeBuilder<ApiResource> builder)
    {
        builder
            .Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired(required: true)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.DisplayName)
            .HasMaxLength(256)
            .IsRequired(required: true)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.Description)
            .HasMaxLength(1024)
            .IsRequired(required: true)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.AllowedAccessTokenSigningAlgorithms)
            .HasMaxLength(1024)
            .IsRequired(required: true);
        builder
            .Property(x => x.Enabled)
            .HasDefaultValue(true)
            .IsRequired(required: true);
        builder
            .Property(x => x.ShowInDiscoveryDocument)
            .HasDefaultValue(true)
            .IsRequired(required: true);
        builder
            .Property(x => x.RequireResourceIndicator)
            .HasDefaultValue(false)
            .IsRequired(required: true);
        builder
            .Property(x => x.Created)
            .IsRequired(required: true)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnAdd();
        builder
            .Property(x => x.Updated)
            .HasValueGenerator<UtcNowDateTimeValueGenerator>()
            .ValueGeneratedOnUpdate();
        builder
            .Property(x => x.LastAccessed)
            .IsRequired(required: false);
        builder
            .Property(x => x.NonEditable)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .OwnsMany(
                x => x.Secrets,
                secrets =>
                {
                    secrets
                        .Property(x => x.Type)
                        .HasMaxLength(256)
                        .IsRequired(required: true);
                    secrets
                        .Property(x => x.Description)
                        .HasMaxLength(Int16.MaxValue)
                        .IsRequired(required: true)
                        .IsUnicode(unicode: true);
                    secrets
                        .Property(x => x.Value)
                        .HasMaxLength(Int16.MaxValue)
                        .IsRequired(required: true)
                        .IsUnicode(unicode: true);
                    secrets
                        .Property(x => x.Created)
                        .IsRequired(required: true)
                        .HasValueGenerator<UtcNowDateTimeValueGenerator>()
                        .ValueGeneratedOnAdd();
                    secrets
                        .Property(x => x.Expiration)
                        .IsRequired(required: false);

                    secrets
                        .HasKey(x => x.Id);

                    secrets
                        .HasIndex(x => x.Type);

                    secrets
                        .WithOwner(x => x.ApiResource)
                        .HasForeignKey(x => x.ApiResourceId);

                    secrets
                        .ToTable(TableNames.ApiResourceSecret, SchemaNames.Identity);
                });
        builder
            .OwnsMany(
                x => x.Scopes,
                scopes =>
                {
                    scopes
                        .Property(x => x.Scope)
                        .HasMaxLength(256)
                        .IsRequired(required: true)
                        .IsUnicode(unicode: true);

                    scopes
                        .HasKey(x => x.Id);

                    scopes
                        .HasIndex(x => x.Scope);

                    scopes
                        .WithOwner(x => x.ApiResource)
                        .HasForeignKey(x => x.ApiResourceId);

                    scopes
                        .ToTable(TableNames.ApiResourceScope, SchemaNames.Identity);
                });
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
                        .HasKey(x => x.Id);

                    claims
                        .HasIndex(x => x.Type);

                    claims
                        .WithOwner(x => x.ApiResource)
                        .HasForeignKey(x => x.ApiResourceId);

                    claims
                        .ToTable(TableNames.ApiResourceClaim, SchemaNames.Identity);
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
                        .IsUnicode(unicode: true)
                        .IsRequired(required: true);

                    properties
                        .HasKey(x => x.Id);

                    properties
                        .WithOwner(x => x.ApiResource)
                        .HasForeignKey(x => x.ApiResourceId);

                    properties
                        .ToTable(TableNames.ApiResourceProperty, SchemaNames.Identity);
                });

        builder
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.Name)
            .IsUnique(unique: false);

        builder
            .ToTable(TableNames.ApiResource, SchemaNames.Identity);
    }
}