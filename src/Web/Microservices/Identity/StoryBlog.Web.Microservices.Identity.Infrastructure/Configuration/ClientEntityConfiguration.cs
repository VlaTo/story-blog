using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Common.Infrastructure.Converters;
using StoryBlog.Web.Microservices.Identity.Application.DependencyInjection.Options;
using StoryBlog.Web.Microservices.Identity.Domain;
using StoryBlog.Web.Microservices.Identity.Domain.Entities;

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Configuration;

internal sealed class ClientEntityConfiguration : IEntityTypeConfiguration<Client>
{
    private readonly ConfigurationStoreOptions storeOptions;

    public ClientEntityConfiguration(ConfigurationStoreOptions storeOptions)
    {
        this.storeOptions = storeOptions;
    }

    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder
            .Property(x => x.Id)
            .IsRequired(required: true);
        builder
            .Property(x => x.Enabled)
            .IsRequired(required: true)
            .HasDefaultValue(true);
        builder
            .Property(x => x.ClientId)
            .IsRequired(required: true);
        builder
            .Property(x => x.ProtocolType)
            .IsRequired(required: true)
            .HasDefaultValue(ProtocolTypes.OpenIdConnect);
        builder
            .Property(x => x.RequireClientSecret)
            .IsRequired(required: true)
            .HasDefaultValue(true);
        builder
            .Property(x => x.ClientName)
            .IsRequired(required: true)
            .HasMaxLength(256)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.UserCodeType)
            .IsRequired(required: true)
            .HasMaxLength(256);
        builder
            .Property(x => x.Description)
            .IsRequired(required: false)
            .HasMaxLength(1024)
            .IsUnicode(unicode: true);
        builder
            .Property(x => x.ClientUri)
            .IsRequired(required: true)
            .HasMaxLength(2083);
        builder
            .Property(x => x.LogoUri)
            .IsRequired(required: false)
            .HasMaxLength(2083);
        builder
            .Property(x => x.RequireConsent)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.AllowRememberConsent)
            .IsRequired(required: true)
            .HasDefaultValue(true);
        builder
            .Property(x => x.AlwaysIncludeUserClaimsInIdToken)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.RequirePkce)
            .IsRequired(required: true)
            .HasDefaultValue(true);
        builder
            .Property(x => x.AllowPlainTextPkce)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.RequireRequestObject)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.AllowAccessTokensViaBrowser)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.FrontChannelLogoutUri)
            .IsRequired(required: false)
            .HasMaxLength(2083);
        builder
            .Property(x => x.FrontChannelLogoutSessionRequired)
            .IsRequired(required: true)
            .HasDefaultValue(true);
        builder
            .Property(x => x.BackChannelLogoutUri)
            .IsRequired(required: false)
            .HasMaxLength(2083);
        builder
            .Property(x => x.BackChannelLogoutSessionRequired)
            .IsRequired(required: true)
            .HasDefaultValue(true);
        builder
            .Property(x => x.AllowOfflineAccess)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.NonEditable)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.PollingInterval)
            .IsRequired(required: false)
            .HasColumnType("bigint");
        builder
            .Property(x => x.IdentityTokenLifetime)
            .IsRequired(required: true)
            .HasDefaultValue(TimeSpan.FromMinutes(5.0d))
            .HasColumnType("bigint");
        builder
            .Property(x => x.CibaLifetime)
            .HasColumnType("bigint")
            .IsRequired(required: true)
            .HasDefaultValue(TimeSpan.FromHours(1.0d));
        builder
            .Property(x => x.AllowedIdentityTokenSigningAlgorithms)
            .IsRequired(required: true);
        builder
            .Property(x => x.AccessTokenLifetime)
            .IsRequired(required: true)
            .HasDefaultValue(TimeSpan.FromHours(1.0d))
            .HasColumnType("bigint");
        builder
            .Property(x => x.AuthorizationCodeLifetime)
            .IsRequired(required: true)
            .HasDefaultValue(TimeSpan.FromMinutes(5.0d))
            .HasColumnType("bigint");
        builder
            .Property(x => x.AbsoluteRefreshTokenLifetime)
            .IsRequired(required: true)
            .HasDefaultValue(TimeSpan.FromDays(30.0d))
            .HasColumnType("bigint");
        builder
            .Property(x => x.SlidingRefreshTokenLifetime)
            .IsRequired(required: true)
            .HasDefaultValue(TimeSpan.FromDays(15.0d))
            .HasColumnType("bigint");
        builder
            .Property(x => x.RefreshTokenUsage)
            .IsRequired(required: true)
            .HasDefaultValue(TokenUsage.OneTimeOnly);
        builder
            .Property(x => x.RefreshTokenExpiration)
            .IsRequired(required: true)
            .HasDefaultValue(TokenExpiration.Absolute);
        builder
            .Property(x => x.AccessTokenType)
            .IsRequired(required: true)
            .HasDefaultValue(AccessTokenType.Jwt);
        builder
            .Property(x => x.EnableLocalLogin)
            .IsRequired(required: true)
            .HasDefaultValue(true);
        builder
            .Property(x => x.ClientClaimsPrefix)
            .IsRequired(required: true)
            .HasDefaultValue("client_")
            .HasMaxLength(256);
        builder
            .Property(x => x.ClientClaimsPrefix)
            .IsRequired(required: true)
            .HasDefaultValue("client_")
            .HasMaxLength(256);
        builder
            .Property(x => x.PairWiseSubjectSalt)
            .IsRequired(required: true)
            .HasMaxLength(256);
        builder
            .Property(x => x.DeviceCodeLifetime)
            .IsRequired(required: true)
            .HasDefaultValue(TimeSpan.FromMinutes(5.0d))
            .HasColumnType("bigint");
        builder
            .Property(x => x.ConsentLifetime)
            .IsRequired(required: false)
            .HasColumnType("bigint");
        builder
            .Property(x => x.AlwaysSendClientClaims)
            .IsRequired(required: true)
            .HasDefaultValue(true);
        builder
            .Property(x => x.IncludeJwtId)
            .IsRequired(required: true)
            .HasDefaultValue(false);
        builder
            .Property(x => x.UserSsoLifetime)
            .IsRequired(required: false)
            .HasColumnType("bigint");
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
            .OwnsMany(
                x => x.AllowedScopes,
                scopes =>
                {
                    scopes
                        .Property(x => x.Scope)
                        .HasMaxLength(256)
                        .IsUnicode(unicode: true)
                        .IsRequired(required: true);

                    scopes
                        .WithOwner(x => x.Client)
                        .HasForeignKey(x => x.ClientId);

                    scopes
                        .HasKey(x => x.Id);
                    scopes
                        .HasIndex(x => x.Scope);

                    scopes
                        .ToTable(TableNames.ClientScope, SchemaNames.Identity);
                }
            );
        builder
            .OwnsMany(
                x => x.ClientSecrets,
                secrets =>
                {
                    secrets
                        .Property(x => x.Value)
                        .HasMaxLength(Int16.MaxValue)
                        .IsUnicode(unicode: true)
                        .IsRequired(required: true);
                    secrets
                        .Property(x => x.Description)
                        .HasMaxLength(Int16.MaxValue)
                        .IsUnicode(unicode: true)
                        .IsRequired(required: true);
                    secrets
                        .Property(x => x.Type)
                        .HasMaxLength(256)
                        .IsRequired(required: true);
                    secrets
                        .Property(x => x.Created)
                        .IsRequired(required: true)
                        .HasValueGenerator<UtcNowDateTimeOffsetValueGenerator>()
                        .ValueGeneratedOnAdd();

                    secrets
                        .WithOwner(x => x.Client)
                        .HasForeignKey(x => x.ClientId);

                    secrets
                        .HasKey(x => x.Id);
                    secrets
                        .HasIndex(x => x.Type);

                    secrets
                        .ToTable(TableNames.ClientSecret, SchemaNames.Identity);
                }
            );
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
                        .WithOwner(x => x.Client)
                        .HasForeignKey(x => x.ClientId);

                    properties
                        .HasKey(x => x.Id);
                    properties
                        .HasIndex(x => x.Key)
                        .IsUnique(unique: false);

                    properties
                        .ToTable(TableNames.ClientProperty, SchemaNames.Identity);
                }
            );
        builder
            .OwnsMany(
                x => x.Claims,
                claims =>
                {
                    claims
                        .Property(x => x.Type)
                        .HasMaxLength(256)
                        .IsRequired(required: true);
                    claims
                        .Property(x => x.Value)
                        .HasMaxLength(Int16.MaxValue)
                        .IsUnicode(unicode: true)
                        .IsRequired(required: true);

                    claims
                        .WithOwner(x => x.Client)
                        .HasForeignKey(x => x.ClientId);
                    claims
                        .HasKey(x => x.Id);
                    claims
                        .HasIndex(x => x.Type)
                        .IsUnique(unique: false);

                    claims
                        .ToTable(TableNames.ClientClaim, SchemaNames.Identity);
                }
            );
        builder
            .OwnsMany(
                x => x.AllowedCorsOrigins,
                cors =>
                {
                    cors
                        .Property(x => x.Origin)
                        .HasMaxLength(2083)
                        .IsRequired(required: true);

                    cors
                        .WithOwner(x => x.Client)
                        .HasForeignKey(x => x.ClientId);

                    cors
                        .HasKey(x => x.Id);
                    cors
                        .HasIndex(x => x.Origin)
                        .IsUnique(unique: false);

                    cors
                        .ToTable(TableNames.ClientCorsOrigin, SchemaNames.Identity);
                }
            );
        builder
            .OwnsMany(
                x => x.AllowedGrantTypes,
                grants =>
                {
                    grants
                        .Property(x => x.GrantType)
                        .HasMaxLength(256)
                        .IsRequired(required: true);

                    grants
                        .WithOwner(x => x.Client)
                        .HasForeignKey(x => x.ClientId);

                    grants
                        .HasKey(x => x.Id);
                    grants
                        .HasIndex(x => x.GrantType)
                        .IsUnique(unique: false);

                    grants
                        .ToTable(TableNames.ClientGrantType, SchemaNames.Identity);
                }
            );
        builder
            .OwnsMany(
                x => x.RedirectUris,
                uris =>
                {
                    uris
                        .Property(x => x.RedirectUri)
                        .HasMaxLength(2083)
                        .IsRequired(required: true);

                    uris
                        .WithOwner(x => x.Client)
                        .HasForeignKey(x => x.ClientId);

                    uris
                        .HasKey(x => x.Id);
                    uris
                        .HasIndex(x => x.RedirectUri)
                        .IsUnique(unique: true);

                    uris
                        .ToTable(TableNames.ClientRedirectUri, SchemaNames.Identity);
                }
            );
        builder
            .OwnsMany(
                x => x.IdentityProviderRestrictions,
                restrictions =>
                {
                    restrictions
                        .Property(x => x.Provider)
                        .HasMaxLength(256)
                        .IsRequired(required: true);

                    restrictions
                        .WithOwner(x => x.Client)
                        .HasForeignKey(x => x.ClientId);

                    restrictions
                        .HasKey(x => x.Id);
                    restrictions
                        .HasIndex(x => x.Provider)
                        .IsUnique(unique: true);

                    restrictions
                        .ToTable(TableNames.ClientIdPRestriction, SchemaNames.Identity);
                }
            );
        builder
            .OwnsMany(
                x => x.PostLogoutRedirectUris,
                uris =>
                {
                    uris
                        .Property(x => x.PostLogoutRedirectUri)
                        .HasMaxLength(2083)
                        .IsRequired(required: true);

                    uris
                        .WithOwner(x => x.Client)
                        .HasForeignKey(x => x.ClientId);

                    uris
                        .HasKey(x => x.Id);
                    uris
                        .HasIndex(x => x.PostLogoutRedirectUri)
                        .IsUnique(unique: true);

                    uris
                        .ToTable(TableNames.ClientPostLogoutRedirectUri, SchemaNames.Identity);
                }
            );

        builder.HasKey(x => x.Id);

        builder.ToTable(storeOptions.Client.Name, storeOptions.Client.Schema);
    }
}