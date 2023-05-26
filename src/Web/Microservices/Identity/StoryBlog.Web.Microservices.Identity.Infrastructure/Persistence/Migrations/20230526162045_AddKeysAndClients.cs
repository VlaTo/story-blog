using System;
using Microsoft.EntityFrameworkCore.Migrations;
using StoryBlog.Web.Microservices.Identity.Domain;

#nullable disable

namespace StoryBlog.Web.Microservices.Identity.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddKeysAndClients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Identity");

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
                    { "Id", "Version", "Created", "Use", "Algorithm", "IsX509Certificate", "DataProtected", "Data" },
                values: new object[]
                {
                    "BD5774334EADCDA65C39FEF2A01AEFB3", 1, "2023-05-26 16:43:42.4669521", "signing", "RS256", false,
                    true,
                    "CfDJ8AoIYmakRP9JrsXlNs9fRz5eom3a0ypQNumXUqONuNrqYQ-tSj7j8uwryY-dQiv7OFYNRmL4Sfq3aL8iPItrTyxEx20amdcUrV5lr969F4xqiqRQsLpXRekg-drsbIndG4nAjnzJ7Wmj6WccpF6fYGs2TawvbCeBrEqp2OLXLqLbN3Vxkbs3bPEfoetX03OYYRcykdemYKcEfr1WKwR_5EbBdu22jsMcIVRKsbwvMHWSeefTJM7V8L1P24Pw71hFhX86GX9X2l0pwvjYuWr130HIIynXEW1D1ZlhOuV1DftU-Zp2qBHLbHqmjXnYSyceveMSYQt_d3mBpmzmCvsQvqSVqTSR9VciYfb5_A1RskrfUhauoJ9qHcVuPfF8KnXCA_ECZ5MGb0zyoKFh1M02kDBTBJfZ1JUZAF89vPovPJr1fRKPJNjFm6__3vBbz9yF5CZ-GVBPzX-79_8Sy9we04GVWHbQPSI_UcrJEqtjxaE2PmazkshhN83nDTaxxjM7ZbiOzUznEijJfBS2U_OYiOCgOG5u86-N2o-97Q4f5fdBVyXqGBT9H2F2RsQ5ZjV-kWnl-kt2fG8IG3ALSsQBRV0WKk-kIJUUP7q83NFwffyrSapL9dbO1uBSeahKZuHCvaWkeHiJM0mc27OZtuNGfI03SIsK_YSiSdcWC_33PMk0mUdvTucL3G3ZTDdPclbLMKge6b_az06TgfH7LxZajiW32Ja_cA3whoUfHlNGm4UTwgjLwF6nILmu4zBHw4kWKGMd0JnVGRztLt8v838yyE2OF05Thin7Y4Wuuc8-Mf20yxwz-_HM8TbsKZ-nCbG-FSkLg9uqImkP1izcnFmIO1bL_JrP4U-_ezI4ytYoBk-5FrmZdW1LgL_MKPGA0pnAaXkbpNPMRdpPngMQlXOAVZIBpYSBMi_RlER7EQNsXwwoYsImIa8XoAo-Ltj5_SIUEBZHmNetFnBNsG9iHXoyJwrV-2IxIWTw5YuuZ0qpD-9VG45WOG3_N4oY-bS-fQPL694F-OzAF7B7daaLUhgAC4z9P4IDQh4nQ7gQ5qSplVrAZI6M7g6CVnZ14TNksjrVimNZk3DY4pRFCMBzRYZSv0BnrhKJ2XcRS4b0e1pZLvUImfmh1rdWvMYztusByXP6StMKO8wOvC2DHKHQTsH2aS74OUu3sYpqQWTCuu6hfJXi_uROOevSpwUCESPkQhYvGJIAlefWTnk2yal2ypBa3e_YknzJGh11efTvdFoa6iuKGKK8IZCKUgT5sKsOQ749-0nYJa-2PtCUKDJNXWuX4V5Pg3nJLYr3PXYZFVG5cSnQYr6MDg8iUdBN5fR2YkJ2wRWI9-NJbC1_Qv4eYigoQFZnzHZwy_mqeenO0VHebJY-pbZv0-v3y_qP_3uRD0I9qMVjV4abhJ9Dct9312oNgsprklxn8vb5ZQpK4Nlw_rwBVTb3M4AX_MjwUB1ne3OcYui3L8QFqINh9ynzqt5wiMYIHF7Am7RHERiU0ZmRigL-8Up_lNsGoBHotiXS9ucQvkezsvCbqTkaJmDMArY34BFud8_Bck9yBWZGjv3Yl93gQDDFc9woGoP2SOEn7_MPi3LsN8uBup6ZJ9jCbWXynnGq072OC2oWRBEfpj2KykCP-KFwQ0Eza3xjF4CKtB5CBA1ZwA2m5HB0hzOY726KHI3LuIUuc5rrZGMubiuiJXaiZiaibsO9TQSbktfxvDQ5cbV3C3vYf8w6bEgL-rbCLgATieIIfdtjR236qBEA5YEIYmt2A3saUjMIbSoabhwMygWTrydt_DEIF3-Wdo5R2O8z6N6erUY4BiY3KoJa_3axsIi31146NxjnUxd1ntNrJHgxlqFVovw39q_JjjVoLFc6gVJkoNihGRiJeuVQg_WfypMiDXUwP1gq-mPBwHUlPjB1JN9TrDL1JMJBtC3tZzYwDo72cd1FWoenBYXLqAQx7LBAHz0UGNGDovENJ5AqmEDdMijG9OKVP3k7v74TRh43eGAHdbF-vDpRzpJcdFC5wJP976-DJBNB_F6432uv-p-CUUJwPX0lzeth6Zap08PyM4xvfYJksOicFrHyNfIaukPZX8xWqPxFb7tOWBwqcMo8oJsl_Rbgx4ThXFNprlaUvXwNdbGGf99yBivjxWAoI352v5AxufjgoL7uli05BDnhES1BzeEcVHJ6TlN2a0X2xfi37UBy86BQupYstG95BGcQxC7tKFv_-0EaNCvlDI7kMT82f-2HuCSun6wqHdU4qsfK7LlMiggFN5J_T1v8TkV23nTJpHmoMvz47e9swx1h0NY680ku5ynZwMTmpwsMiG_ujrNpltrVw1yY3w6VKevv0bDjUYhpVzZrduwteeafsLHRyIZHTO6cHRCFaNNdshvYHyLsoH1GlYlzh0-Ur3kQS_TpvVqAFtFMw1Jjz1JBQ3PUE4YTcxgv__YSJ2E4-YF4e7rKB0Lxw8rO2fuY"
                }
            );

            #endregion
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Keys",
                schema: "Identity");

            migrationBuilder.DropTable(
                name: "Clients",
                schema: "Identity");
        }
    }
}
