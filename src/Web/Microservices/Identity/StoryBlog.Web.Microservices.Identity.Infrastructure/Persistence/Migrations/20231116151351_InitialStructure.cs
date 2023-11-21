using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

            migrationBuilder.CreateTable(
                name: "ApiResources",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    AllowedAccessTokenSigningAlgorithms = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    RequireResourceIndicator = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastAccessed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    NonEditable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiScopes",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(156)", maxLength: 156, nullable: true),
                    Description = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: true),
                    Required = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Emphasize = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastAccessed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    NonEditable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    ProtocolType = table.Column<string>(type: "text", nullable: false, defaultValue: "oidc"),
                    RequireClientSecret = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ClientName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ClientUri = table.Column<string>(type: "character varying(2083)", maxLength: 2083, nullable: false),
                    LogoUri = table.Column<string>(type: "character varying(2083)", maxLength: 2083, nullable: true),
                    RequireConsent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AllowRememberConsent = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RequirePkce = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    AllowPlainTextPkce = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    RequireRequestObject = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    FrontChannelLogoutUri = table.Column<string>(type: "character varying(2083)", maxLength: 2083, nullable: true),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    BackChannelLogoutUri = table.Column<string>(type: "character varying(2083)", maxLength: 2083, nullable: true),
                    BackChannelLogoutSessionRequired = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    AllowOfflineAccess = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IdentityTokenLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 3000000000L),
                    AllowedIdentityTokenSigningAlgorithms = table.Column<string>(type: "text", nullable: false),
                    AccessTokenLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 36000000000L),
                    AuthorizationCodeLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 3000000000L),
                    ConsentLifetime = table.Column<long>(type: "bigint", nullable: true),
                    AbsoluteRefreshTokenLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 25920000000000L),
                    SlidingRefreshTokenLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 12960000000000L),
                    RefreshTokenUsage = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "boolean", nullable: false),
                    RefreshTokenExpiration = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    AccessTokenType = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    EnableLocalLogin = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IncludeJwtId = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    AlwaysSendClientClaims = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ClientClaimsPrefix = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false, defaultValue: "client_"),
                    PairWiseSubjectSalt = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserSsoLifetime = table.Column<long>(type: "bigint", nullable: true),
                    UserCodeType = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DeviceCodeLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 3000000000L),
                    CibaLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 36000000000L),
                    PollingInterval = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastAccessed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    NonEditable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceFlowCodes",
                schema: "Identity",
                columns: table => new
                {
                    DeviceCode = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserCode = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: false),
                    SubjectId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SessionId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ClientId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: true),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Expiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Data = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "IdentityProviders",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Scheme = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: true),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Type = table.Column<string>(type: "character varying(256)", unicode: false, maxLength: 256, nullable: false),
                    Properties = table.Column<string>(type: "character varying(32767)", unicode: false, maxLength: 32767, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastAccessed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    NonEditable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityProviders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityResources",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Required = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Emphasize = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Updated = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    NonEditable = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityResources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Keys",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Use = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Algorithm = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    IsX509Certificate = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DataProtected = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Data = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SubjectId = table.Column<string>(type: "text", nullable: false),
                    SessionId = table.Column<string>(type: "text", nullable: false),
                    ClientId = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: true),
                    CreationTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Expiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    ConsumedTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Data = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServerSideSessions",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Scheme = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    SubjectId = table.Column<string>(type: "text", nullable: false),
                    SessionId = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Renewed = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Expires = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Data = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerSideSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiResourceClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApiResourceId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResourceClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiResourceClaims_ApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalSchema: "Identity",
                        principalTable: "ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiResourceProperties",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApiResourceId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResourceProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiResourceProperties_ApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalSchema: "Identity",
                        principalTable: "ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiResourceScopes",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Scope = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ApiResourceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiResourceScopes_ApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalSchema: "Identity",
                        principalTable: "ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiResourceSecrets",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ApiResourceId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false),
                    Value = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false),
                    Expiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiResourceSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiResourceSecrets_ApiResources_ApiResourceId",
                        column: x => x.ApiResourceId,
                        principalSchema: "Identity",
                        principalTable: "ApiResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiScopeClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScopeId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopeClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiScopeClaims_ApiScopes_ScopeId",
                        column: x => x.ScopeId,
                        principalSchema: "Identity",
                        principalTable: "ApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApiScopeProperties",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScopeId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopeProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiScopeProperties_ApiScopes_ScopeId",
                        column: x => x.ScopeId,
                        principalSchema: "Identity",
                        principalTable: "ApiScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientClaims_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Identity",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientCorsOrigins",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Origin = table.Column<string>(type: "character varying(2083)", maxLength: 2083, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientCorsOrigins_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Identity",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientGrantTypes",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GrantType = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientGrantTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientGrantTypes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Identity",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientIdPRestrictions",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Provider = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientIdPRestrictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientIdPRestrictions_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Identity",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientPostLogoutRedirectUris",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PostLogoutRedirectUri = table.Column<string>(type: "character varying(2083)", maxLength: 2083, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPostLogoutRedirectUris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientPostLogoutRedirectUris_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Identity",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientProperties",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientProperties_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Identity",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientRedirectUris",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RedirectUri = table.Column<string>(type: "character varying(2083)", maxLength: 2083, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRedirectUris", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientRedirectUris_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Identity",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientScopes",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Scope = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ClientId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientScopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientScopes_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Identity",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientSecrets",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false),
                    Value = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false),
                    Expiration = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientSecrets_Clients_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "Identity",
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityResourceClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityResourceId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityResourceClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityResourceClaims_IdentityResources_IdentityResourceId",
                        column: x => x.IdentityResourceId,
                        principalSchema: "Identity",
                        principalTable: "IdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IdentityResourceProperties",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityResourceId = table.Column<int>(type: "integer", nullable: false),
                    Key = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "character varying(32767)", maxLength: 32767, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityResourceProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityResourceProperties_IdentityResources_IdentityResour~",
                        column: x => x.IdentityResourceId,
                        principalSchema: "Identity",
                        principalTable: "IdentityResources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Group = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Modified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                schema: "Identity",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Identity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                schema: "Identity",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApiResourceClaims_ApiResourceId",
                schema: "Identity",
                table: "ApiResourceClaims",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiResourceClaims_Type",
                schema: "Identity",
                table: "ApiResourceClaims",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ApiResourceProperties_ApiResourceId",
                schema: "Identity",
                table: "ApiResourceProperties",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiResources_Name",
                schema: "Identity",
                table: "ApiResources",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ApiResourceScopes_ApiResourceId",
                schema: "Identity",
                table: "ApiResourceScopes",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiResourceScopes_Scope",
                schema: "Identity",
                table: "ApiResourceScopes",
                column: "Scope");

            migrationBuilder.CreateIndex(
                name: "IX_ApiResourceSecrets_ApiResourceId",
                schema: "Identity",
                table: "ApiResourceSecrets",
                column: "ApiResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiResourceSecrets_Type",
                schema: "Identity",
                table: "ApiResourceSecrets",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ApiScopeClaims_ScopeId",
                schema: "Identity",
                table: "ApiScopeClaims",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiScopeClaims_Type",
                schema: "Identity",
                table: "ApiScopeClaims",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ApiScopeProperties_ScopeId",
                schema: "Identity",
                table: "ApiScopeProperties",
                column: "ScopeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiScopes_Id",
                schema: "Identity",
                table: "ApiScopes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ClientClaims_ClientId",
                schema: "Identity",
                table: "ClientClaims",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientClaims_Type",
                schema: "Identity",
                table: "ClientClaims",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCorsOrigins_ClientId",
                schema: "Identity",
                table: "ClientCorsOrigins",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientCorsOrigins_Origin",
                schema: "Identity",
                table: "ClientCorsOrigins",
                column: "Origin");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGrantTypes_ClientId",
                schema: "Identity",
                table: "ClientGrantTypes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientGrantTypes_GrantType",
                schema: "Identity",
                table: "ClientGrantTypes",
                column: "GrantType");

            migrationBuilder.CreateIndex(
                name: "IX_ClientIdPRestrictions_ClientId",
                schema: "Identity",
                table: "ClientIdPRestrictions",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientIdPRestrictions_Provider",
                schema: "Identity",
                table: "ClientIdPRestrictions",
                column: "Provider",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientPostLogoutRedirectUris_ClientId",
                schema: "Identity",
                table: "ClientPostLogoutRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPostLogoutRedirectUris_PostLogoutRedirectUri",
                schema: "Identity",
                table: "ClientPostLogoutRedirectUris",
                column: "PostLogoutRedirectUri",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientProperties_ClientId",
                schema: "Identity",
                table: "ClientProperties",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientProperties_Key",
                schema: "Identity",
                table: "ClientProperties",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRedirectUris_ClientId",
                schema: "Identity",
                table: "ClientRedirectUris",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientRedirectUris_RedirectUri",
                schema: "Identity",
                table: "ClientRedirectUris",
                column: "RedirectUri",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientScopes_ClientId",
                schema: "Identity",
                table: "ClientScopes",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientScopes_Scope",
                schema: "Identity",
                table: "ClientScopes",
                column: "Scope");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSecrets_ClientId",
                schema: "Identity",
                table: "ClientSecrets",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientSecrets_Type",
                schema: "Identity",
                table: "ClientSecrets",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceFlowCodes_ClientId_DeviceCode",
                schema: "Identity",
                table: "DeviceFlowCodes",
                columns: new[] { "ClientId", "DeviceCode" });

            migrationBuilder.CreateIndex(
                name: "IX_IdentityProviders_Type",
                schema: "Identity",
                table: "IdentityProviders",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityResourceClaims_IdentityResourceId",
                schema: "Identity",
                table: "IdentityResourceClaims",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_IdentityResourceProperties_IdentityResourceId",
                schema: "Identity",
                table: "IdentityResourceProperties",
                column: "IdentityResourceId");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_Key",
                schema: "Identity",
                table: "PersistedGrants",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                schema: "Identity",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Identity",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServerSideSessions_Key",
                schema: "Identity",
                table: "ServerSideSessions",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                schema: "Identity",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                schema: "Identity",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                schema: "Identity",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Identity",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Identity",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            SeedIdentityData(migrationBuilder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiResourceClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiResourceProperties",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiResourceScopes",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiResourceSecrets",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiScopeClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiScopeProperties",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ClientClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ClientCorsOrigins",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ClientGrantTypes",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ClientIdPRestrictions",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ClientPostLogoutRedirectUris",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ClientProperties",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ClientRedirectUris",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ClientScopes",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ClientSecrets",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "DeviceFlowCodes",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityProviders",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityResourceClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityResourceProperties",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Keys",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "PersistedGrants",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "RoleClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ServerSideSessions",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserClaims",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserLogins",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "UserTokens",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiResources",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiScopes",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityResources",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Identity");
        }
    }
}
