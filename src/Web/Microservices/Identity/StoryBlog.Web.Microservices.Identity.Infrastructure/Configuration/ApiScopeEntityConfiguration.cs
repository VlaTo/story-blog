using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Domain;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Configuration;

internal sealed class ApiScopeEntityConfiguration : IEntityTypeConfiguration<ApiScope>
{
    private readonly ConfigurationStoreOptions storeOptions;

    public ApiScopeEntityConfiguration(ConfigurationStoreOptions storeOptions)
    {
        this.storeOptions = storeOptions;
    }

    public void Configure(EntityTypeBuilder<ApiScope> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired(required: true)
            .HasMaxLength(256)
            .IsUnicode(unicode: false);
        builder
            .Property(x => x.Description)
            .IsRequired(required: false)
            .HasMaxLength(Int16.MaxValue)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.DisplayName)
            .IsRequired(required: false)
            .HasMaxLength(156)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.Enabled)
            .IsRequired(required: true)
            .HasDefaultValue(true);
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
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
            .ValueGeneratedOnAdd();
        builder
            .Property(x => x.Updated)
            .IsRequired(required: false)
            .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
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
                        .WithOwner(x => x.Scope)
                        .HasForeignKey(x => x.ScopeId);

                    properties
                        .ToTable(TableNames.ApiScopeProperty, SchemaNames.Identity);
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
                        .WithOwner(x => x.Scope)
                        .HasForeignKey(x => x.ScopeId);

                    claims
                        .ToTable(TableNames.ApiScopeClaim, SchemaNames.Identity);
                });

        builder
            .HasIndex(x => x.Id);

        builder
            .ToTable(TableNames.ApiScope, SchemaNames.Identity);
    }
}