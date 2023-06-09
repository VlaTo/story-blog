using System;
using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Security;
using Microsoft.EntityFrameworkCore.Migrations;
using StoryBlog.Web.Microservices.Identity.Application;
using StoryBlog.Web.Microservices.Identity.Application.Storage;
using StoryBlog.Web.Microservices.Identity.Domain;

#nullable disable

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityEntities : Migration
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    AllowedAccessTokenSigningAlgorithms = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    RequireResourceIndicator = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NonEditable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(156)", maxLength: 156, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: true),
                    Required = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Emphasize = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NonEditable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProtocolType = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "oidc"),
                    RequireClientSecret = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ClientName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    ClientUri = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: false),
                    LogoUri = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: true),
                    RequireConsent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AllowRememberConsent = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RequirePkce = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AllowPlainTextPkce = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RequireRequestObject = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AllowAccessTokensViaBrowser = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FrontChannelLogoutUri = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: true),
                    FrontChannelLogoutSessionRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    BackChannelLogoutUri = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: true),
                    BackChannelLogoutSessionRequired = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    AllowOfflineAccess = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IdentityTokenLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 3000000000L),
                    AllowedIdentityTokenSigningAlgorithms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccessTokenLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 36000000000L),
                    AuthorizationCodeLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 3000000000L),
                    ConsentLifetime = table.Column<long>(type: "bigint", nullable: true),
                    AbsoluteRefreshTokenLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 25920000000000L),
                    SlidingRefreshTokenLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 12960000000000L),
                    RefreshTokenUsage = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "bit", nullable: false),
                    RefreshTokenExpiration = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    AccessTokenType = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    EnableLocalLogin = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    IncludeJwtId = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    AlwaysSendClientClaims = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ClientClaimsPrefix = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, defaultValue: "client_"),
                    PairWiseSubjectSalt = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserSsoLifetime = table.Column<long>(type: "bigint", nullable: true),
                    UserCodeType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DeviceCodeLifetime = table.Column<long>(type: "bigint", nullable: false, defaultValue: 3000000000L),
                    CibaLifetime = table.Column<long>(type: "bigint", nullable: true),
                    PollingInterval = table.Column<long>(type: "bigint", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NonEditable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
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
                    DeviceCode = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserCode = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "IdentityProviders",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Scheme = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: true),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Type = table.Column<string>(type: "varchar(256)", unicode: false, maxLength: 256, nullable: false),
                    Properties = table.Column<string>(type: "varchar(max)", unicode: false, maxLength: 32767, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastAccessed = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NonEditable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    Required = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Emphasize = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ShowInDiscoveryDocument = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NonEditable = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
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
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Use = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Algorithm = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    IsX509Certificate = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DataProtected = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConsumedTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServerSideSessions",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Scheme = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    SubjectId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SessionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Renewed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Data = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServerSideSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiResourceClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiResourceId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiResourceId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Scope = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ApiResourceId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiResourceId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScopeId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScopeId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false)
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Group = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientClaims",
                schema: "Identity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origin = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrantType = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PostLogoutRedirectUri = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RedirectUri = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Scope = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ClientId = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityResourceId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityResourceId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", maxLength: 32767, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityResourceProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IdentityResourceProperties_IdentityResources_IdentityResourceId",
                        column: x => x.IdentityResourceId,
                        principalSchema: "Identity",
                        principalTable: "IdentityResources",
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
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

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
                name: "IX_ServerSideSessions_Key",
                schema: "Identity",
                table: "ServerSideSessions",
            column: "Key");

            #region Users

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[]
                {
                    "Id", "IsActive", "Created", "Modified", "UserName", "NormalizedUserName", "Email",
                    "NormalizedEmail", "EmailConfirmed", "PasswordHash", "SecurityStamp", "ConcurrencyStamp",
                    "PhoneNumber", "PhoneNumberConfirmed", "TwoFactorEnabled", "LockoutEnd", "LockoutEnabled",
                    "AccessFailedCount"
                },
                values: new object[]
                {
                    "1b2696b7-7605-49be-b59c-ab577f6c1cc0", true, "2023-06-08 08:40:34.0273908",
                    "2023-06-08 08:40:34.0348363", "guest@storyblog.net", "GUEST@STORYBLOG.NET", "guest@storyblog.net",
                    "GUEST@STORYBLOG.NET", true,
                    "AQAAAAIAAYagAAAAEP2omrptRd+Y6ldZrmgtyGyiS5lCKnBNVSZFOxOioe798g708HWi818ueedmCfjfKg==",
                    "JUBOLHW6CITA22TQG4QV4FCUDQDYZCKA", "e96c8397-8d72-4ffb-9cf4-246d5fa2f36f", "8-800-444-55-66", true,
                    false, null, true, 0
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[]
                {
                    "Id", "Description", "Created", "Modified", "Name", "NormalizedName", "ConcurrencyStamp"
                },
                values: new object[]
                {
                    "9e63463a-67ab-4bb7-a019-c151ea74a05b", "Blog Viewer", "2023-06-08 12:34:00.7029025", null,
                    "Permissions.Blogs.View", "PERMISSIONS.BLOGS.VIEW", "7389d70d-c988-42f3-9712-5d1c607d60a7"
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[]
                {
                    "UserId", "RoleId"
                },
                values: new object[]
                {
                    "1b2696b7-7605-49be-b59c-ab577f6c1cc0", "9e63463a-67ab-4bb7-a019-c151ea74a05b"
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetUserClaims",
                columns: new[]
                {
                    "UserId", "ClaimType", "ClaimValue"
                },
                values: new object[]
                {
                    "1b2696b7-7605-49be-b59c-ab577f6c1cc0",
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", "Guest"
                }
            );

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[]
                {
                    "RoleId", "ClaimType", "ClaimValue", "Discriminator", "Description", "Group", "Created",
                    "Modified"
                },
                values: new object[]
                {
                    "9e63463a-67ab-4bb7-a019-c151ea74a05b",
                    "http://schemas.xmlsoap.org/ws/2009/09/identity/claims/actor", "Viewer",
                    "IdentityRoleClaim<string>", null, null, null, null
                }
            );

            #endregion
            
            #region Identity Clients

            migrationBuilder.InsertData(
                table: TableNames.Client,
                schema: SchemaNames.Identity,
                columns: new[]
                {
                    "ClientId", "ClientName", "Description", "ClientUri", "Enabled", "FrontChannelLogoutUri",
                    "BackChannelLogoutUri", "AllowedIdentityTokenSigningAlgorithms", "ConsentLifetime",
                    "UpdateAccessTokenClaimsOnRefresh", "PairWiseSubjectSalt", "UserSsoLifetime", "UserCodeType",
                    "CibaLifetime", "PollingInterval", "Created"
                },
                values: new object[]
                {
                    "288849a891664840975fa7992f247947", "Sample Blog", "Sample Blog Client", "http://localhost:5035",
                    true, "http://localhost:5035/logout", "http://localhost:5035/logout-callback", "RS256", 150000000,
                    false, "k3jr23khk2e4jJKH2hjxbdw_jdTr", 500000000000, "token", 150000000, 15000000000,
                    new DateTime(2023, 05, 26, 11, 43, 39)
                }
            );

            #endregion

            #region Identity Clients Keys

            migrationBuilder.InsertData(
                table: TableNames.Keys,
                schema: SchemaNames.Identity,
                columns: new[]
                {
                    "Id", "Version", "Created", "Use", "Algorithm", "IsX509Certificate", "DataProtected", "Data"
                },
                values: new object[]
                {
                    "BD5774334EADCDA65C39FEF2A01AEFB3", 1, "2023-05-26 16:43:42.4669521", "signing", "RS256", false,
                    true,
                    "CfDJ8AoIYmakRP9JrsXlNs9fRz5eom3a0ypQNumXUqONuNrqYQ-tSj7j8uwryY-dQiv7OFYNRmL4Sfq3aL8iPItrTyxEx20"+
                    "amdcUrV5lr969F4xqiqRQsLpXRekg-drsbIndG4nAjnzJ7Wmj6WccpF6fYGs2TawvbCeBrEqp2OLXLqLbN3Vxkbs3bPEfoe"+
                    "tX03OYYRcykdemYKcEfr1WKwR_5EbBdu22jsMcIVRKsbwvMHWSeefTJM7V8L1P24Pw71hFhX86GX9X2l0pwvjYuWr130HII"+
                    "ynXEW1D1ZlhOuV1DftU-Zp2qBHLbHqmjXnYSyceveMSYQt_d3mBpmzmCvsQvqSVqTSR9VciYfb5_A1RskrfUhauoJ9qHcVu"+
                    "PfF8KnXCA_ECZ5MGb0zyoKFh1M02kDBTBJfZ1JUZAF89vPovPJr1fRKPJNjFm6__3vBbz9yF5CZ-GVBPzX-79_8Sy9we04G"+
                    "VWHbQPSI_UcrJEqtjxaE2PmazkshhN83nDTaxxjM7ZbiOzUznEijJfBS2U_OYiOCgOG5u86-N2o-97Q4f5fdBVyXqGBT9H2"+
                    "F2RsQ5ZjV-kWnl-kt2fG8IG3ALSsQBRV0WKk-kIJUUP7q83NFwffyrSapL9dbO1uBSeahKZuHCvaWkeHiJM0mc27OZtuNGf"+
                    "I03SIsK_YSiSdcWC_33PMk0mUdvTucL3G3ZTDdPclbLMKge6b_az06TgfH7LxZajiW32Ja_cA3whoUfHlNGm4UTwgjLwF6n"+
                    "ILmu4zBHw4kWKGMd0JnVGRztLt8v838yyE2OF05Thin7Y4Wuuc8-Mf20yxwz-_HM8TbsKZ-nCbG-FSkLg9uqImkP1izcnFm"+
                    "IO1bL_JrP4U-_ezI4ytYoBk-5FrmZdW1LgL_MKPGA0pnAaXkbpNPMRdpPngMQlXOAVZIBpYSBMi_RlER7EQNsXwwoYsImIa"+
                    "8XoAo-Ltj5_SIUEBZHmNetFnBNsG9iHXoyJwrV-2IxIWTw5YuuZ0qpD-9VG45WOG3_N4oY-bS-fQPL694F-OzAF7B7daaLU"+
                    "hgAC4z9P4IDQh4nQ7gQ5qSplVrAZI6M7g6CVnZ14TNksjrVimNZk3DY4pRFCMBzRYZSv0BnrhKJ2XcRS4b0e1pZLvUImfmh"+
                    "1rdWvMYztusByXP6StMKO8wOvC2DHKHQTsH2aS74OUu3sYpqQWTCuu6hfJXi_uROOevSpwUCESPkQhYvGJIAlefWTnk2yal"+
                    "2ypBa3e_YknzJGh11efTvdFoa6iuKGKK8IZCKUgT5sKsOQ749-0nYJa-2PtCUKDJNXWuX4V5Pg3nJLYr3PXYZFVG5cSnQYr"+
                    "6MDg8iUdBN5fR2YkJ2wRWI9-NJbC1_Qv4eYigoQFZnzHZwy_mqeenO0VHebJY-pbZv0-v3y_qP_3uRD0I9qMVjV4abhJ9Dc"+
                    "t9312oNgsprklxn8vb5ZQpK4Nlw_rwBVTb3M4AX_MjwUB1ne3OcYui3L8QFqINh9ynzqt5wiMYIHF7Am7RHERiU0ZmRigL-"+
                    "8Up_lNsGoBHotiXS9ucQvkezsvCbqTkaJmDMArY34BFud8_Bck9yBWZGjv3Yl93gQDDFc9woGoP2SOEn7_MPi3LsN8uBup6"+
                    "ZJ9jCbWXynnGq072OC2oWRBEfpj2KykCP-KFwQ0Eza3xjF4CKtB5CBA1ZwA2m5HB0hzOY726KHI3LuIUuc5rrZGMubiuiJX"+
                    "aiZiaibsO9TQSbktfxvDQ5cbV3C3vYf8w6bEgL-rbCLgATieIIfdtjR236qBEA5YEIYmt2A3saUjMIbSoabhwMygWTrydt_"+
                    "DEIF3-Wdo5R2O8z6N6erUY4BiY3KoJa_3axsIi31146NxjnUxd1ntNrJHgxlqFVovw39q_JjjVoLFc6gVJkoNihGRiJeuVQ"+
                    "g_WfypMiDXUwP1gq-mPBwHUlPjB1JN9TrDL1JMJBtC3tZzYwDo72cd1FWoenBYXLqAQx7LBAHz0UGNGDovENJ5AqmEDdMij"+
                    "G9OKVP3k7v74TRh43eGAHdbF-vDpRzpJcdFC5wJP976-DJBNB_F6432uv-p-CUUJwPX0lzeth6Zap08PyM4xvfYJksOicFr"+
                    "HyNfIaukPZX8xWqPxFb7tOWBwqcMo8oJsl_Rbgx4ThXFNprlaUvXwNdbGGf99yBivjxWAoI352v5AxufjgoL7uli05BDnhE"+
                    "S1BzeEcVHJ6TlN2a0X2xfi37UBy86BQupYstG95BGcQxC7tKFv_-0EaNCvlDI7kMT82f-2HuCSun6wqHdU4qsfK7LlMiggF"+
                    "N5J_T1v8TkV23nTJpHmoMvz47e9swx1h0NY680ku5ynZwMTmpwsMiG_ujrNpltrVw1yY3w6VKevv0bDjUYhpVzZrduwteea"+
                    "fsLHRyIZHTO6cHRCFaNNdshvYHyLsoH1GlYlzh0-Ur3kQS_TpvVqAFtFMw1Jjz1JBQ3PUE4YTcxgv__YSJ2E4-YF4e7rKB0"+
                    "Lxw8rO2fuY"
                }
            );

            #endregion

            #region Identity Clients grant types

            migrationBuilder.InsertData(
                table: TableNames.ClientGrantType,
                schema: SchemaNames.Identity,
                columns: new[]
                {
                    "GrantType", "ClientId"
                },
                values: new object[]
                {
                    GrantType.AuthorizationCode, 1
                }
            );
            migrationBuilder.InsertData(
                table: TableNames.ClientGrantType,
                schema: SchemaNames.Identity,
                columns: new[]
                {
                    "GrantType", "ClientId"
                },
                values: new object[]
                {
                    GrantType.ClientCredentials, 1
                }
            );

            #endregion

            #region Identity Clients CORS

            migrationBuilder.InsertData(
                table: TableNames.ClientCorsOrigin,
                schema: SchemaNames.Identity,
                columns: new[]
                {
                    "Origin", "ClientId"
                },
                values: new object[]
                {
                    "http://localhost:5030", 1
                }
            );

            #endregion

            #region Client Scopes

            migrationBuilder.InsertData(
                table: TableNames.ClientScope,
                schema: SchemaNames.Identity,
                columns: new[]
                {
                    "Scope", "ClientId"
                },
                values: new object[][]
                {
                    new object[] { "openid", 1 },
                    new object[] { "profile", 1 }
                }
            );

            #endregion

            #region Identity Clients Redirect Uris

            migrationBuilder.InsertData(
                table: TableNames.ClientRedirectUri,
                schema: SchemaNames.Identity,
                columns: new[]
                {
                    "RedirectUri", "ClientId"
                },
                values: new object[]
                {
                    "http://localhost:5035/authentication/login-callback", 1
                }
            );

            #endregion

            #region Identity Clients Secrets

            migrationBuilder.InsertData(
                table: TableNames.ClientSecret,
                schema: SchemaNames.Identity,
                columns: new[]
                {
                    "ClientId", "Description", "Value", "Expiration", "Type", "Created"
                },
                values: new object[]
                {
                    1, "StoryBlog basic secret", "l356jtlk3j5l6tk3j6ltk3j5;6tlk345j6l34k",
                    new DateTime(2030, 1, 1, 12, 00, 00), SecretTypes.SharedSecret,
                    new DateTime(2023, 5, 29, 12, 30, 00)
                }
            );

            #endregion

            #region Clients Properties

            migrationBuilder.InsertData(
                table: TableNames.ClientProperty,
                schema: SchemaNames.Identity,
                columns: new[]
                {
                    "ClientId", "Key", "Value"
                },
                values: new object[]
                {
                    1, "Profile", "SPA"
                }
            );

            #endregion

            #region Identity Resources

            migrationBuilder.InsertData(
                table: TableNames.IdentityResource,
                schema: SchemaNames.Identity,
                columns: new[]
                {
                    "Enabled", "Name", "DisplayName", "Description", "Required", "Emphasize", "ShowInDiscoveryDocument",
                    "Created", "Updated", "NonEditable"
                },
                values: new object[][]
                {
                    new object[]
                    {
                        1, "profile", "SimpleBlog OpenID Profile resource", "SimpleBlog OpenID Profile resource", false,
                        false, true, "2022-07-11 09:05:50.7431775", null, false
                    },
                    new object[]
                    {
                        1, "openid", "SimpleBlog OpenID General resource", "SimpleBlog OpenID General resource", false,
                        false, true, "2022-07-11 09:05:50.7431775", null, false
                    }

                }
            );

            #endregion
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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

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
                name: "ServerSideSessions",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiResources",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "ApiScopes",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "IdentityResources",
                schema: "Identity");
        }
    }
}
